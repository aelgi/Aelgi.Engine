module Aelgi.Engine.DataAccess.Neo4j.Services.DBConnector

open System
open Neo4j.Driver
open Aelgi.Engine.Core.IServices.Database

let rec private convertRecords<'T> (records: IRecord list) (key: string) =
    if records.IsEmpty then []
    else
        let item = records.Head.Values.As<'T>()
        item::(convertRecords records.Tail key)
        
type DatabaseResult (keys: string list, records: IRecord list) =
    
    interface IDatabaseResult with
        override this.Keys with get() = keys
        
        member this.RenderColumn<'T> key =
            convertRecords<'T> records key

type DatabaseAdapter (uri: string, username: string, password: string) =
    let driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password))
    
    let rec iterateResults (cursor: IResultCursor) =
        async {
            let! isMore = cursor.FetchAsync() |> Async.AwaitTask
            if isMore then
                let! nextItem = iterateResults cursor
                return cursor.Current::nextItem
            else return []
        }
    
    interface IDatabaseAdapter with
        member this.ExecuteQueryWithResult (query: string) (parameters: obj) =
            let sesh = driver.AsyncSession()
            async {
                let! tempRes = sesh.RunAsync(query, parameters) |> Async.AwaitTask
                let! records = iterateResults tempRes
                
                let! keys = tempRes.KeysAsync() |> Async.AwaitTask
                
                do! sesh.CloseAsync() |> Async.AwaitTask
                
                return DatabaseResult (keys |> Array.toList, records |> List.rev) :> IDatabaseResult
            }
            
        member this.ExecuteQuery (query: string) (parameters: obj) =
            let sesh = driver.AsyncSession()
            async {
                let! tempRes = sesh.RunAsync(query, parameters) |> Async.AwaitTask
                
                do! sesh.CloseAsync() |> Async.AwaitTask
            }

let connect uri username password =
    DatabaseAdapter (uri, username, password)
    :> IDatabaseAdapter
    
let testConnection (adapter: IDatabaseAdapter) () =
    adapter.ExecuteQuery "MATCH (n) RETURN distinct labels(n)" null