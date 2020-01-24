module Aelgi.Engine.Core.Network.Converter

open System
open System.IO
open System.Text

let private encoding = Encoding.UTF8

let messageToByteArray (message: string) =
    let messageBytes = encoding.GetBytes message
    
    let completeMsg =
        messageBytes.Length
        |> (+) 4
        |> Array.zeroCreate<byte>
        
    let sizeBytes =
        messageBytes.Length
        |> BitConverter.GetBytes
    
    sizeBytes.CopyTo(completeMsg, 0)
    messageBytes.CopyTo(completeMsg, 4)
    
    completeMsg
    
let streamToMessage (stream: Stream) =
    async {
        let sizeBytes = Array.zeroCreate<byte> 4
        do! stream.ReadAsync(sizeBytes, 0, 4) |> Async.AwaitTask |> Async.Ignore
        
        let messageSize = BitConverter.ToInt32(sizeBytes, 0)
        let messageBytes = Array.zeroCreate<byte> messageSize
        do! stream.ReadAsync(messageBytes, 0, messageSize) |> Async.AwaitTask |> Async.Ignore
        
        return encoding.GetString messageBytes
    }