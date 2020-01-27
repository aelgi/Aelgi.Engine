module Aelgi.Engine.Server.Handler.Delegators.UserDelegator

open Aelgi.Engine.Core.Models
open Aelgi.Engine.Core.Message

let createHandler (creator: (UserModel -> Async<UserModel option>)) (user: UserModel) =
    let result =
        user
        |> creator
        |> Async.RunSynchronously
        
    match result with
    | Some _ -> "created" |> Success |> Ok
    | None -> Failure |> Ok