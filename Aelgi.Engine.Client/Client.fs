module Aelgi.Engine.Client

open System.Net.Sockets
open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.Network

type Connection =
    {
        Host: string
        Port: int
    }
    
let connect (host: string) (port: int) =
    { Host = host; Port = port; }
    
let private openStream (connection: Connection) =
    let client = new TcpClient(connection.Host, connection.Port)
    client.NoDelay <- true
    
    client
    
let sendMessageWithResponse (message: ClientMessage) (connection: Connection) =
    use client = openStream connection
    use stream = client.GetStream()
    
    Encoder.encodeClient message
    |> (fun x -> stream.Write(x, 0, x.Length))
    
    Encoder.decodeServer stream
    
let sendMessage (message: ClientMessage) (connection: Connection) =
    sendMessageWithResponse message connection |> ignore
    connection