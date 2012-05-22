// Learn more about F# at http://fsharp.net

open System
open MassTransit
open MassTransit.NLogIntegration
open NLog.Config

// the below types are in the global namespace, under the "NamespaceLess" module. A module = static class in c#.

// this is the interface that we're going to implement and subscribe to
[<AllowNullLiteral>]
type TestMessage =
  abstract member Message : string with get

[<AllowNullLiteral>]
type TestMessageExplIntf(msg) =
  interface TestMessage with
    member x.Message = msg

type TestMessageExplIntfFsharp(msg) =
  interface TestMessage with
    member x.Message = msg

type TestMessageRecord3 =
  { MyMessage : string }
  interface TestMessage with
    member x.Message = x.MyMessage

type TestMessageDiscUn =
   | OnlyOption of string
   interface TestMessage with
     member x.Message = match x with | OnlyOption(msg) -> msg

let initBus () =
  ServiceBusFactory.New(fun sbc ->
    sbc.UseNLog()
    sbc.UseRabbitMqRouting()
    sbc.SetPurgeOnStartup(true) |> ignore
    sbc.ReceiveFrom("rabbitmq://localhost/MassTransit.FSharpInterop.Outbound"))

let initReceiver() =
  ServiceBusFactory.New(fun sbc ->
    sbc.UseNLog()
    sbc.UseRabbitMqRouting()
    sbc.SetPurgeOnStartup(true) |> ignore
    sbc.ReceiveFrom("rabbitmq://localhost/MassTransit.FSharpInterop.Inbound")
    sbc.Subscribe(fun s -> s.Handler(fun (msg : TestMessage) -> printfn "Got %s. Success!" msg.Message) |> ignore))

let waitFor<'a> recv (f : 'a -> unit) = ()
let read () = Console.ReadKey(true) |> ignore
let wait i = 
  printfn "waiting..."
  System.Threading.Thread.Sleep(i * 1000)

[<EntryPoint>]
let main args =

  printfn "Starting..."
  use sb = initBus ()
  use recv = initReceiver ()
  use ep = sb.GetEndpoint(Uri("rabbitmq://localhost/MassTransit.FSharpInterop.Inbound"))
  
  printfn "\n\npress a key to send a 'normal impl message'"
  read ()
  let msg1 = "As much C# as possible!"
  ep.Send<TestMessage>(TestMessageExplIntf(msg1))
  wait 3

  printfn "\n\npress a key to send a 'normal impl message w/o nulls'"
  read ()
  let msg1' = "A little less C#ish"
  ep.Send<TestMessage>(TestMessageExplIntfFsharp(msg1'))
  wait 3

  printfn "\n\npretty much same as above but with cast to determine type"
  read ()
  let msg1'' = "Mucho C#"
  ep.Send(TestMessageExplIntf(msg1'') :> TestMessage)
  wait 3
  
  printfn "\n\npress a key to send an 'in-place interface impl' msg"
  read ()
  let msg2 = "With anonymous interface impl"
  ep.Send({ new TestMessage with
              member x.Message = msg2 })
  wait 3

  printfn "\n\npress a key to send a record set implementing an interface"
  read ()
  let msg3 = "With a record"
  ep.Send({ MyMessage = msg3 })
  wait 3

  printfn "\n\npress a key to send a discriminated union implementing an interface"
  read ()
  let msg4 = "With a discriminated union!"
  ep.Send(OnlyOption(msg4))
  wait 3

  printfn "Waiting..."
  Console.ReadKey(true) |> ignore
  0