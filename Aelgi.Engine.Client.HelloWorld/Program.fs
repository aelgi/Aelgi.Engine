
open Aelgi.Engine
open Aelgi.Engine.Client
open Aelgi.Engine.Core.Message
open System

[<EntryPoint>]
let main argv =
    
    let message =
        "Hello world"
        |> ClientMessage.Ping
    
    let result =
        Client.connect "localhost" 7707
        |> Client.sendMessageWithResponse message
        
    let timeDiff =
        Client.connect "localhost" 7707
        |> Client.sendMessageWithResponse (DateTime.UtcNow |> ClientMessage.Time)
    
    printfn "Hello World from F#!"
    0 // return an integer exit code
