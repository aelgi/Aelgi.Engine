module Aelgi.Engine.Server.Handler.Server

open Aelgi.Engine.Core
open System.Net
open System.Net.Sockets
open System.Threading

exception UnableToQueueException

let private handleIncomingRequest (client: TcpClient) (a: obj) =
    let request =
        client.GetStream()
        |> Network.Converter.streamToMessage
        |> Async.RunSynchronously
        
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