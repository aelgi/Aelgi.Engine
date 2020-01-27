module Aelgi.Engine.Core.IServices.Delegators

open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.Models
open System

type SimpleDelegator<'T> = 'T -> ServerMessage

type PingDelegator = SimpleDelegator<string>
type TimeDelegator = SimpleDelegator<DateTime>

type UserCreateDelegator = SimpleDelegator<UserModel>