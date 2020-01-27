namespace Aelgi.Engine.Core.IServices.Database

type QueryRunner = string -> unit

type IDatabaseResult =
    abstract Keys : string list with get
    abstract member FetchColumn: string -> Map<string, obj> list
    
type IDatabaseAdapter =
    abstract member ExecuteQueryWithResult : string -> obj -> Async<IDatabaseResult option>
    abstract member ExecuteQuery : string -> obj -> Async<unit option>