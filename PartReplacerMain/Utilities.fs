namespace Replacer

module Utilities =

    let printOptionLine x =
        let number, category = x
        Console.cyan <| sprintf """    [%i] %s""" number category
        printfn ""

    let displayOptions =
        let boltCategories = Details.BoltCategories
        let indexer = [1 .. 6]
        printfn ""
        let sx = List.zip  indexer boltCategories
        sx
        |> List.iter printOptionLine
        printfn ""
        printfn ""
        Console.cyan <| sprintf @"Select material by pressing keys [1,2,3,4,5,6]"
        printfn ""
        0

