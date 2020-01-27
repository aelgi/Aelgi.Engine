namespace Aelgi.Engine.Core.Message

open System

type Response =
    | Success of string
    | Failure
    
module Response =
    let isSuccess =
        function
            | Success s -> true
            | _ -> false

type ServerMessage =
    | Ok of Response
    | Pong of string
    | Time of double
    
type ClientMessage =
    | Ping of string
    | Time of DateTime
    | User of UserMessage
