module Aelgi.Engine.Core.IServices.Delegators

open Aelgi.Engine.Core.Message
open System

type PingDelegator = string -> ServerMessage
type TimeDelegator = DateTime -> ServerMessage