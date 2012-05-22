namespace MassTransit.FSharpInterop.SampleMessages

type CreateBraznik =
  abstract member Age : int
  abstract member Name : string

type CreateBraznikImpl(age : int, name : string) =
  member x.Age = age
  member x.Name = name

type BraznikCreated =
  interface
  end