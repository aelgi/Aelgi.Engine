module Aelgi.Engine.Server.Handler.Server

open Aelgi.Engine.Core.Network
open System.Net
open System.Net.Sockets
open System.Threading

exception UnableToQueueException

let private handleIncomingRequest (client: TcpClient) (a: obj) =
    use stream = client.GetStream()
    
    let request =
        stream
        |> Encoder.decodeClient
        
    let result =
        request
        |> Delegator.processMessage
        |> Encoder.encodeServer
        
    stream.Write(result, 0, result.Length)
        
    client.Close()        

let listen (port: int) =
    let ipAddress = IPAddress.Any
    let server = new TcpListener(ipAddress, port)
    server.Start()
    
    while true do
        let client = server.AcceptTcpClient()
        
        client
        |> handleIncomingRequest
        |> ThreadPool.QueueUserWorkItem
        |> function
            | false -> raise UnableToQueueException
            | _ -> ()
    
    ()