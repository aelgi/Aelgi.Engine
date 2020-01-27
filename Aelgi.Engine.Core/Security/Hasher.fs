module Aelgi.Engine.Core.Security.Hasher

open System
open System.Security.Cryptography
open System.Text

let generateSalt () =
    let salt =
        128 / 8
        |> Array.zeroCreate
        
    use rng = RandomNumberGenerator.Create ()
    rng.GetBytes(salt)
    
    Convert.ToBase64String(salt)
    
let rec private hash (algo: HashAlgorithm) (iterations: int) (input: byte array) =
    if iterations <= 0 then input
    else
        input
        |> algo.ComputeHash
        |> hash algo (iterations - 1)
    
let generatePassword (salt: string) (password: string) =
    use sha = SHA512.Create()
    
    let input =
        password
        |> Encoding.UTF8.GetBytes
        |> hash sha 10000
        |> Convert.ToBase64String
      
    input + salt
    |> Encoding.UTF8.GetBytes
    |> hash sha 10000
    |> Convert.ToBase64String
    
let generateSaltAndPassword (password: string) =
    let salt = generateSalt ()
    let password = generatePassword salt password
    (salt, password)