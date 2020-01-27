module Aelgi.Engine.Server.Handler.Delegator

open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.IServices.Delegators

type Delegator = ClientMessage -> ServerMessage

type Handlers =
    {
        pingDelegator: PingDelegator
        timeDelegator: TimeDelegator
        
        userCreateDelegator: UserCreateDelegator
    }
    
let private processUserMessage (handlers: Handlers) (message: UserMessage) =
    match message with
    | Create u -> handlers.userCreateDelegator u

let processMessage (handlers: Handlers) (message: ClientMessage) =
    match message with
    | Ping s -> handlers.pingDelegator s
    | Time t -> handlers.timeDelegator t
    | User u -> processUserMessage handlers u