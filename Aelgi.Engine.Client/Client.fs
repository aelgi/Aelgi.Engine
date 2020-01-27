namespace Aelgi.Engine.Client

open Aelgi.Engine.Core.IServices.Connection
open System.Net.Sockets
open Aelgi.Engine.Core.Message
open Aelgi.Engine.Core.Network

type ConnectionAdapter (host: string, port: int) =
//    let hostInfo = Dns.GetHostEntry host
//    let hostAddress = hostInfo.AddressList |> Array.head
//    let endpoint =
//        if host = "localhost" then IPEndPoint(IPAddress.Loopback, port)
//        else new IPEndPoint(hostAddress, port)
    
    interface IConnectionAdapter with
        member this.OpenStream () =
            let client = new TcpClient(host, port)
            client.NoDelay <- false
            client.GetStream()
            
        member this.Close () =
            ()
//            client.Close()

type SendMessageWithResponse = ClientMessage -> ServerMessage
type SendMessage = ClientMessage -> unit

[<AutoOpen>]
module Connector =
    let connect (host: string) (port: int) =
        ConnectionAdapter (host, port)
        :> IConnectionAdapter
        
    let sendMessageWithResponse (connection: IConnectionAdapter) (message: ClientMessage) =
        use stream = connection.OpenStream()
        
        Encoder.encodeClient message
        |> (fun x -> stream.Write(x, 0, x.Length))
        
        Encoder.decodeServer stream
        
    let sendMessage (connection: IConnectionAdapter) (message: ClientMessage) =
        sendMessageWithResponse connection message |> ignore