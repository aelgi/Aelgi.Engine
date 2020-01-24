// Learn more about F# at http://fsharp.org

open Aelgi.Engine.Server.Handler
open System

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    
    Server.listen 7707
    
    0 // return an integer exit code
