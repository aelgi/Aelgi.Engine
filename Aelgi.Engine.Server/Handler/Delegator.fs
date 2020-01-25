module Aelgi.Engine.Server.Handler.Delegator

open System
open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.IServices.Delegators

type Delegator = ClientMessage -> ServerMessage

type Handlers =
    {
        pingDelegator: PingDelegator
        timeDelegator: TimeDelegator
    }

let processMessage (handlers: Handlers) (message: ClientMessage) =
    match message with
    | Ping s -> handlers.pingDelegator s
    | Time t -> handlers.timeDelegator t