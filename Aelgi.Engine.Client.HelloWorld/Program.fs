
open Aelgi.Engine.Client
open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.Models
open System

[<EntryPoint>]
let main argv =
    
    let message =
        "Hello world"
        |> ClientMessage.Ping
        
    let client = connect "localhost" 7707
    let connection = sendMessageWithResponse client
    
    let userModel = {
        UserModel.Email = "test@gmail.com"
        Username = "hello"
        Password = "world"
    }
    
    let result =
        userModel
        |> createUser connection
    
//    let result =
//        "Hello world"
//        |> ClientMessage.Ping
//        |> sendMessage
//        
//    let timeDiff =
//        DateTime.UtcNow
//        |> ClientMessage.Time
//        |> sendMessage
    
    printfn "Hello World from F#!"
    0 // return an integer exit code
