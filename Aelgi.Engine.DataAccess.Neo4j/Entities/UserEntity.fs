namespace Aelgi.Engine.DataAccess.Neo4j.Entities

open Aelgi.Engine.Core.Models

type UserEntity =
    {
        Username: string
        Password: string
        Salt: string
        
        Email: string
    }
    
module UserEntity =
    let convertFromDictionary (dict: Map<string, obj>) =
        let findOrDefault key =
            dict
            |> Map.tryFind key
            |> function
                | Some value -> downcast value
                | None -> ""
            
        {
            UserEntity.Email = findOrDefault "Email"
            Username = findOrDefault "Username"
            Salt = findOrDefault "Salt"
            Password = findOrDefault "Password"
        }
        
    let convertToModel (entity: UserEntity) =
        {
            UserModel.Email = entity.Email
            Username = entity.Username
            Password = ""
        }