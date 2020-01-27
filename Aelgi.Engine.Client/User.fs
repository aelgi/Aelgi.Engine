namespace Aelgi.Engine.Client

open Aelgi.Engine.Core.Models
open Aelgi.Engine.Core.Message

[<AutoOpen>]
module User =
    let createUser (connection: SendMessageWithResponse) (user: UserModel) =
        user
        |> Create
        |> User
        |> connection
        |> function
            | Ok res -> Response.isSuccess res
            | _ -> false