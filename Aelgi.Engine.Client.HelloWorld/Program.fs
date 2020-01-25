
open Aelgi.Engine
open Aelgi.Engine.Client
open Aelgi.Engine.Core.Message
open System

[<EntryPoint>]
let main argv =
    
    let message =
        "Hello world"
        |> ClientMessage.Ping
        
    let client = Client.connect "localhost" 7707
    let sendMessage = Client.sendMessageWithResponse client
    
    let result =
        "Hello world"
        |> ClientMessage.Ping
        |> sendMessage
        
    let timeDiff =
        DateTime.UtcNow
        |> ClientMessage.Time
        |> sendMessage
    
    printfn "Hello World from F#!"
    0 // return an integer exit code
