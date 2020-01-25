namespace Aelgi.Engine.Core.Message

open System

type ServerMessage =
    | Ok of unit
    | Pong of string
    | Time of double
    
type ClientMessage =
    | Ping of string
    | Time of DateTime
