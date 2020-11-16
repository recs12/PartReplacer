
namespace Helpers

open System

module Utilities =



    let tablePath : string = @"J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\table.json"
    let fastenersPath : string = @"J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\fasteners.json"

    let zip s1 s2 = List.zip s1 s2 |> List.ofSeq

    let printOptionLine x = printfn """[%i] %s""" (fst x) (snd x)

    let displayOptions =
        let material : list<string> = [ "imperial zinc"; "metric zinc"; "imperial ss-304"; "metric ss-304"; "imperial ss-316"; "metric ss-316" ]
        let indexer = [1 .. 6]
        printfn ""
        let sx = zip indexer material
        sx
        |> List.iter printOptionLine
        printfn ""
        printfn @"Select material with keys [1,2,3,4,5,6] or press ?"
        printfn @"if you would like to check the current conversion table."
        0





module User =

    let version : string = "0.0.1"
    let author : string = "recs"
    let update : string = "2020-11-04"

    let displayDetails author version update =
        printfn "PartReplacer  --author:%s --version:%s --last-update :%s" author version update
        printfn @"Would you like to replace the fasteners in this assembly ?"
        printfn @"Select the items you want to change and press y/[Y] to proceed:"
        0


module Switcher =

    let getUserChoice : string=
        (*Get the user to choice the material for the part replacement.*)
        let switcher (choice: string) : string=
            match choice with
            | "1" -> "imperial zinc"
            | "2" -> "metric zinc"
            | "3" -> "imperial ss-304"
            | "4" -> "metric ss-304"
            | "5" -> "imperial ss-316"
            | "6" -> "metric ss-316"
            | "?" -> "?"
            | _ -> ""
        let response:string = Console.ReadLine()
        let material =  switcher response
        material