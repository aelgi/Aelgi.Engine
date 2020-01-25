namespace Aelgi.Engine.Core.IServices.Connection

open System.Net.Sockets

type IConnectionAdapter =
    abstract member OpenStream : unit -> NetworkStream
    abstract member Close : unit -> unit