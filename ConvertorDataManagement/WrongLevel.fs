namespace Tools

module Level =

    let WrongLevel =
        Console.red   <| sprintf  "[!] Item not in the current assembly level."
        printfn ""
        |> ignore
