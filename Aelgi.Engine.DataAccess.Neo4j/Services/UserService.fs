namespace Aelgi.Engine.DataAccess.Neo4j.Services

open System
open Aelgi.Engine.DataAccess.Neo4j.Entities
open Aelgi.Engine.Core.IServices.Database
open Aelgi.Engine.Core.Models
open Aelgi.Engine.Core.Security

module UserService =
    let createUser (adapter: IDatabaseAdapter) (user: UserModel) =
        let (salt, password) =
            user.Password
            |> Hasher.generateSaltAndPassword
        
        let entity =
            {
                UserEntity.Username = user.Username
                Password = password
                Salt = salt
                Email = user.Email
            }
            
        async {
            let! result =
                {| Entity = entity |}
                |> adapter.ExecuteQueryWithResult "CREATE (u:User) SET u = $Entity RETURN u"
                
            let user =
                match result with
                | Some u -> u.FetchColumn "u" |> List.head |> UserEntity.convertFromDictionary |> UserEntity.convertToModel |> Some
                | None -> None
                
            return user
        }
        
    let fetchUsers (adapter: IDatabaseAdapter) () =
        async {
            let! result =
                adapter.ExecuteQueryWithResult "MATCH (u:User) RETURN u" null
                
            let values =
                result
                |> Option.map (fun x -> x.FetchColumn "u")
                |> Option.map (List.map UserEntity.convertFromDictionary)
                |> Option.map (List.map UserEntity.convertToModel)
            return values
        }