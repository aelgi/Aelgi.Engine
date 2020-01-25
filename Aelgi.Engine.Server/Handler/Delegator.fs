module Aelgi.Engine.Server.Handler.Delegator

open System
open Aelgi.Engine.Core.Message

let processMessage (message: ClientMessage) =
    match message with
    | Ping s -> ServerMessage.Pong s
    | Time t -> (DateTime.UtcNow.Subtract t).TotalMilliseconds |> ServerMessage.Time