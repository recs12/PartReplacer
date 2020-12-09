namespace Warnings

open Helpers

module Selection =
    let WrongSelection =
        Console.red   <| sprintf  "[!] No active selection: Select some items before running the macro again."
        printfn ""|> ignore