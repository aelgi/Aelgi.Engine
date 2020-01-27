module Aelgi.Engine.Server.Handler.TimeDelegator

open Aelgi.Engine.Core.Message
open System

type DateTimeLookup = unit -> DateTime

let handler (d: DateTimeLookup) (receivedDateTime: DateTime) =
    d().Subtract(receivedDateTime).TotalMilliseconds
    |> ServerMessage.Time