namespace Aelgi.Engine.Server

type NeoSettings () =
    member val URI = "" with get, set
    member val Username = "" with get, set
    member val Password = "" with get, set

type AppSettings () =
    member val Neo4j = NeoSettings () with get, set