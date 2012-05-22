module ProblematicOverloads

open MassTransit
open MassTransit.NLogIntegration
open MassTransit.FSharpInterop.SampleMessages
open System

type SampleConsumer() =
  interface Consumes<CreateBraznik>.Context with
    member x.Consume ctx =
      ctx.Respond({ new BraznikCreated })

let initOut () = 
  ServiceBusFactory.New(fun sbc -> 
      sbc.UseNLog()
      sbc.UseRabbitMqRouting()
      sbc.SetPurgeOnStartup(true) |> ignore
      sbc.ReceiveFrom(Uri("rabbitmq://localhost/MassTransit.FSharpInterop.Inbound"))
      sbc.Subscribe(fun s -> s.Consumer<SampleConsumer>() |> ignore ))

let initIn () =
  ServiceBusFactory.New(fun sbc ->
    sbc.UseNLog()
    sbc.UseRabbitMqRouting()
    sbc.SetPurgeOnStartup(true) |> ignore
    sbc.ReceiveFrom("rabbitmq://localhost/MassTransit.FSharpInterop.Inbound")
    sbc.Subscribe(fun s -> s.Handler(fun (msg : BraznikCreated) -> printfn "Got %A. Success!" msg) |> ignore))

[<EntryPoint>]
let main args =
  use out = initOut ()
  use inb = initIn ()
  inb.Publish<CreateBraznik>(CreateBraznikImpl(56, "Ture Grevesson"), (fun ctx -> 
    ctx.SetConversationId("Communism")))
  Console.ReadKey(true) |> ignore
  0