module Aelgi.Engine.Server.Handler.Server

open Aelgi.Engine.Core.Network
open System.Net
open System.Net.Sockets
open System.Threading

exception UnableToQueueException

let private handleIncomingRequest (delegator: Delegator.Delegator) (client: TcpClient) (_: obj) =
    use stream = client.GetStream()
    
    let request =
        stream
        |> Encoder.decodeClient
        
    let result =
        request
        |> delegator
        |> Encoder.encodeServer
        
    stream.Write(result, 0, result.Length)
        
    client.Close()        

let listen (delegationHandler: Delegator.Delegator) (port: int) =
    let ipAddress = IPAddress.Any
    let server = new TcpListener(ipAddress, port)
    server.Start()
    
    let requestHandler = handleIncomingRequest delegationHandler
    
    while true do
        let client = server.AcceptTcpClient()
        
        client
        |> requestHandler
        |> ThreadPool.QueueUserWorkItem
        |> function
            | false -> raise UnableToQueueException
            | _ -> ()
    
    ()