﻿
namespace Helpers

open System
open FSharp.Json


module User =

    let displayDetails author version update =
        printfn "PartReplacer :"
        printfn "====================================================================="
        printfn " --author: %s --version: %s --last-update: %s" author version update
        printfn "---------------------------------------------------------------------"
        printfn @" Would you like to replace the fasteners in this assembly? Select the"
        printfn @" items you want to change and press y/[Y] to proceed:"
        |>ignore


module Utilities =

    let zip s1 s2 = List.zip s1 s2 |> List.ofSeq

    let printOptionLine x =
        let number, category = x
        printfn """    [%i] %s""" number category

    let displayOptions =
        let boltCategories = Details.BoltCategories
        let indexer = [1 .. 6]
        printfn ""
        let sx = zip indexer boltCategories
        sx
        |> List.iter printOptionLine
        printfn ""
        printfn @" Select material with keys [1,2,3,4,5,6] or press [?]"
        printfn @" if you would like to check the current conversion table."
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


    type FastenerDetails = {
            JdeNumber: string
            Revision: string
            Filename: string
    }

    type ItemCollection = FastenerDetails list

    let getReplacementPartDetails jdeNumber =

        let json :string = System.IO.File.ReadAllText(Details.dataFileFasteners)

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


module TableConversion =

    type Table = Map<string, string>

    type ConversionChartList = Map<string, Table>

    let getEquivalentByTypeMaterial (jdeNumber:string) (material) =

        let json = System.IO.File.ReadAllText(Details.dataFileTables)

        let deserializedTableData = Json.deserialize<ConversionChartList> json

        let getTable (collectionsChart: ConversionChartList) (jdeNum:string)=
            collectionsChart.TryGetValue jdeNum

        let tableStatus, table = getTable deserializedTableData jdeNumber

        let partnumber =
            match tableStatus with
            | true ->  table.[material]
            | false -> ""

        let part = partnumber
        part


module Chart=


    let displayChart jde chart =

        let boltCategories = Details.BoltCategories

        let rec search dict key =
            match dict with
            | [] -> ""
            | (k, v) :: _ when k = key -> v
            | _ :: tl -> search tl key

        let signet jde key = if jde = key then ">" else " "
        let equality jde key = if jde = key then "==" else "<>"

        (*Display of the chart to user bellow.*)

        printfn "match %10s with" jde
        let wx = List.zip [1..6] boltCategories
        wx |> List.iter (fun wx ->
            printfn "%4i|%s %-16s -> %-10s %s" (fst wx) (signet jde (search chart (snd wx))) (snd wx) (search chart (snd wx)) (equality jde (search chart (snd wx))))
