
namespace Helpers

open System
open FSharp.Json

module User =

    let version : string = "0.0.2"
    let author : string = "recs"
    let update : string = "2020-11-04"

    let displayDetails author version update =
        printfn "PartReplacer :"
        printfn "--author: %s --version: %s --last-update : %s" author version update
        printfn @"Would you like to replace the fasteners in this assembly ?"
        printfn @"Select the items you want to change and press y/[Y] to proceed:"
        0

module Utilities =

    let zip s1 s2 = List.zip s1 s2 |> List.ofSeq

    let printOptionLine x = printfn """    [%i] %s""" (fst x) (snd x)

    let displayOptions =
        let material : list<string> = [ "imperial zinc"; "metric zinc"; "imperial ss-304"; "metric ss-304"; "imperial ss-316"; "metric ss-316" ]
        let indexer = [1 .. 6]
        printfn ""
        let sx = zip indexer material
        sx
        |> List.iter printOptionLine
        printfn ""
        printfn @"Select material with keys [1,2,3,4,5,6] or press [?]"
        printfn @"if you would like to check the current conversion table."
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

module Fasteners =

    let Inputfilename : string = @"J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\fasteners2.json"

    type FastenerDetails = {
            JdeNumber: string
            Revision: string
            Filename: string
    }

    type ItemCollection = FastenerDetails list

    let getReplacementPartDetails jdeNumber =

        let json :string = System.IO.File.ReadAllText(Inputfilename)

        let deserialized: ItemCollection = Json.deserialize<ItemCollection> json

        let matching x =
            match x with
                | Some item -> (item.JdeNumber, item.Revision, item.Filename)
                | None -> ("","","")

        let searchDetails (collectionsPart: ItemCollection) (jdeNum: string) =
            collectionsPart
                |> List.tryFind (fun j -> j.JdeNumber = jdeNum)
                |> matching

        let item = searchDetails deserialized jdeNumber

        item
