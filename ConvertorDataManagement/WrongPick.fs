namespace Warnings

open Helpers

module Pick =
    let WrongPick =
        Console.red   <| sprintf  "[!] Wrong pick! Try again but this time stick to the choice displayed."
        printfn ""
        |> ignore
