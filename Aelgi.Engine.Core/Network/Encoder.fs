module Aelgi.Engine.Core.Network.Encoder

open System.IO
open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.Network
open FSharp.Json

type Message =
    | ServerMessage of ServerMessage
    | ClientMessage of ClientMessage

let encodeClient (message: ClientMessage) =
    Json.serialize message
    |> Converter.messageToByteArray
    
let encodeServer (message: ServerMessage) =
    Json.serialize message
    |> Converter.messageToByteArray
    
let decodeServer (incoming: Stream) =
    let message =
        incoming
        |> Converter.streamToMessage
        |> Async.RunSynchronously
    
    message |> Json.deserialize<ServerMessage>
    
let decodeClient (incoming: Stream) =
    let message =
        incoming
        |> Converter.streamToMessage
        |> Async.RunSynchronously
        
    message
    |> Json.deserialize<ClientMessage>