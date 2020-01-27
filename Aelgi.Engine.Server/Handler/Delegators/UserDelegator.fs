module Aelgi.Engine.Server.Handler.Delegators.UserDelegator

open Aelgi.Engine.Core.Models
open Aelgi.Engine.Core.Message

let createHandler (creator: (UserModel -> Async<UserModel>)) (user: UserModel) =
    let result =
        user
        |> creator
        |> Async.RunSynchronously
        
    "created"
    |> Success
    |> Ok