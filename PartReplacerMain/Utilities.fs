namespace Replacer

module Utilities =


    let printMaterialLine x =
        let number, category = x
        printfn """    [%i] %s""" number category


    let displayChoices =
        let boltCategories = Details.BoltCategories
        let indexer = [1 .. 6]
        printfn ""
        let sx = List.zip  indexer boltCategories
        sx
        |> List.iter printMaterialLine
        printfn ""
        printfn ""
        printfn @"Select material by pressing keys [1,2,3,4,5,6]"
        0

