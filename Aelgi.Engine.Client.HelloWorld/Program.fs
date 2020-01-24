
open Aelgi.Engine.Client
open System

[<EntryPoint>]
let main argv =
    
    Connector.connect "localhost" 7707 |> Async.RunSynchronously
    
    printfn "Hello World from F#!"
    0 // return an integer exit code
