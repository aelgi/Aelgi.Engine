module Aelgi.Engine.Server.Handler.TimeDelegator

open Aelgi.Engine.Core.Message
open System

let handler (d: DateTime) =
    DateTime.UtcNow.Subtract(d).TotalMilliseconds
    |> ServerMessage.Time