namespace Aelgi.Engine.DataAccess.Neo4j.Services

open Neo4j.Driver
open Aelgi.Engine.Core.IServices.Database

        
type DatabaseResult (keys: string list, records: IRecord list) =
//    let rec fetchRecords (recs: IRecord list) (key: string) =
//        if recs.IsEmpty then []
//        else
//            let node = records.Head.[0].As<INode>()
//            node.Properties::(fetchRecords records.Tail key)
//    
//    member this.ConvertRecords<'T when 'T : (new : unit -> 'T)> (records: IRecord list) (key: string) =
//        if records.IsEmpty then []
//        else
//            let node = records.Head.[0].As<INode>()
//            let model =
//                node.Properties
//            let item = records.Head.[0].As<'T>().As<'T>()
//            item::(this.ConvertRecords records.Tail key)
    
    interface IDatabaseResult with
        override this.Keys with get() = keys
        
        member this.FetchColumn key =
            records
            |> List.map (fun x -> x.[key].As<INode>().Properties)
            |> List.map (fun x -> x |> Seq.map (fun y -> (y.Key, y.Value)))
            |> List.map Map.ofSeq
        
type DatabaseAdapter (uri: string, username: string, password: string) =
    let driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password))
    
    interface IDatabaseAdapter with
        member this.ExecuteQueryWithResult (query: string) (parameters: obj) =
            let sesh = driver.AsyncSession()
            async {
                let! tempRes = sesh.RunAsync(query, parameters) |> Async.AwaitTask
                let! records = tempRes.ToListAsync() |> Async.AwaitTask
                
                let! keys = tempRes.KeysAsync() |> Async.AwaitTask
                
                do! sesh.CloseAsync() |> Async.AwaitTask
                
                return DatabaseResult (keys |> Array.toList, records |> Seq.rev |> Seq.toList) :> IDatabaseResult
            }
            
        member this.ExecuteQuery (query: string) (parameters: obj) =
            let sesh = driver.AsyncSession()
            async {
                let! tempRes = sesh.RunAsync(query, parameters) |> Async.AwaitTask
                
                do! sesh.CloseAsync() |> Async.AwaitTask
            }

[<AutoOpen>]
module DBConnector =
    let connect uri username password =
        DatabaseAdapter (uri, username, password)
        :> IDatabaseAdapter
        
    let testConnection (adapter: IDatabaseAdapter) () =
        adapter.ExecuteQuery "MATCH (n) RETURN distinct labels(n)" null