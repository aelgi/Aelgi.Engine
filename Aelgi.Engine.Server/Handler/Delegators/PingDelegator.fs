module Aelgi.Engine.Server.Handler.PingDelegator

open Aelgi.Engine.Core.Message

let handler (message: string) =
    message
    |> ServerMessage.Pong