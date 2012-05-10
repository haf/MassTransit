// Learn more about F# at http://fsharp.net

open System
open MassTransit
open MassTransit.NLogIntegration
open NLog.Config

[<AllowNullLiteral>]
type TestMessage =
  abstract member Message : string with get

[<AllowNullLiteral>]
type TestMessageImpl(msg) =
  interface TestMessage with
    member x.Message = msg

let initBus () =
  ServiceBusFactory.New(fun sbc ->
    sbc.UseNLog()
    sbc.UseRabbitMqRouting()
    sbc.SetPurgeOnStartup(true) |> ignore
    sbc.ReceiveFrom("rabbitmq://localhost/MassTransit.FSharpInterop"))

let initReceiver() =
  ServiceBusFactory.New(fun sbc ->
    sbc.UseRabbitMqRouting()
    sbc.UseNLog()
    sbc.SetPurgeOnStartup(true) |> ignore
    sbc.ReceiveFrom("rabbitmq://localhost/test_queue")
    sbc.Subscribe(fun s -> s.Handler(fun (msg : TestMessage) -> printfn "Got %s." msg.Message) |> ignore))

let waitFor<'a> recv (f : 'a -> unit) =
  ()

[<EntryPoint>]
let main args =

  printfn "Starting..."
  use sb = initBus ()
  use recv = initReceiver ()
  use ep = sb.GetEndpoint(Uri("rabbitmq://localhost/test_queue"))
  
  printfn "press a key to send a 'normal impl message'"
  Console.ReadKey(true) |> ignore
  let msg1 = "As one would do in C#"
  ep.Send<TestMessage>(TestMessageImpl(msg1))

  printfn "press a key to send a 'casted msg'"
  Console.ReadKey(true) |> ignore
  let msg2 = "By casting"
  ep.Send(TestMessageImpl(msg2) :> TestMessage)
  
  printfn "press a key to send an 'in-place interface impl' msg"
  Console.ReadKey(true) |> ignore
  let msg3 = "With anonymous interface impl"
  ep.Send({ new TestMessage with
              member x.Message = msg3 })

  printfn "Waiting..."
  0