module Aelgi.Engine.Client.Connector

open Aelgi.Engine.Core
open System.Net.Sockets

let connect (host: string) (port: int) =
    async {
        let client = new TcpClient(host, port)
        client.NoDelay <- true
        
        let messageBytes = Network.Converter.messageToByteArray "Hello World!"
        
        use stream = client.GetStream()
        do! stream.WriteAsync(messageBytes, 0, messageBytes.Length) |> Async.AwaitTask
     
        client.Close()
    }