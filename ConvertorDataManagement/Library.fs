
namespace Helpers

open System
open FSharp.Json


module Blank =
    let line = printfn ""


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

    open Blank

    let printOptionLine x =
        let number, category = x
        printfn """    [%i] %s""" number category

    let displayOptions =
        let boltCategories = Details.BoltCategories
        let indexer = [1 .. 6]
        Blank.line
        let sx = List.zip  indexer boltCategories
        sx
        |> List.iter printOptionLine
        Blank.line
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


module Chart =


    let displayChart jde tab material =

        let Keys(map: Map<'K,'V>) =
            seq {
                for KeyValue(key,value) in map do
                    yield (key,value)
            } |> List.ofSeq

        let chart = Keys tab

        let boltCategories = Details.BoltCategories

        let rec findIn dict key =
            match dict with
            | [] -> ""
            | (k, v) :: _ when k = key -> v
            | _ :: tl -> findIn tl key

        let arrow material key =
            match (material = key) with
            | true -> ">"
            | false -> " "

        let equality jde key =
            match (jde = key) with
            | true -> "(=)"
            | false -> " "


        let displaylines line =
            for index, category in line do
            printfn "%10i|%s  %-16s   ->   %-10s  %s" index (arrow material category) category (findIn chart category) (equality jde (findIn chart category))


        printfn "--- match %8s with ---" jde
        Blank.line
        let zippedIndexAndCategories = List.zip [1..6] boltCategories
        displaylines zippedIndexAndCategories
        Blank.line


module TableConversion =

    open Chart

    type Table = Map<string, string>

    type ConversionChartList = Map<string, Table>

    let getEquivalentByTypeMaterial (jdeNumber:string) (material) =

        let json = System.IO.File.ReadAllText(Details.dataFileTables)

        let deserializedTableData = Json.deserialize<ConversionChartList> json

        let getTable (collectionsChart: ConversionChartList) (jdeNum:string)=
            collectionsChart.TryGetValue jdeNum

        let Success, table = getTable deserializedTableData jdeNumber

        let partnumber =
            if (Success && material <> "?") then table.[material]
            else            ""

        let part = partnumber

        Chart.displayChart part table material

        part


//module CacheContent =

//    type ProperySets() =
//        interface SolidEdgeFileProperties.PropertySets with
//            member this.Application = raise (System.NotImplementedException())
//            member this.Close() = raise (System.NotImplementedException())
//            member this.Count = raise (System.NotImplementedException())
//            member this.CreateCustonPropertySet() = raise (System.NotImplementedException())
//            member this.GetEnumerator() = raise (System.NotImplementedException())
//            member this.GetFamilyOfAssemblyMemberNames(fileName, memberCount, memberNames) = raise (System.NotImplementedException())
//            member this.IsFileFamilyOfAssembly(fileName, bFamilyOfAssembly) = raise (System.NotImplementedException())
//            member this.IsFileWeldmentAssembly(fileName, bWeldmentAssembly) = raise (System.NotImplementedException())
//            member this.Item
//                with get (index) = raise (System.NotImplementedException())
//            member this.Parent = raise (System.NotImplementedException())
//            member this.Save() = raise (System.NotImplementedException())
//            member this.Open(a,b) = raise (System.NotImplementedException())


//    type Properties() =
//        interface SolidEdgeFileProperties.Properties with
//            member this.Add(name, value) = raise (System.NotImplementedException())
//            member this.Application = raise (System.NotImplementedException())
//            member this.Count = raise (System.NotImplementedException())
//            member this.GetEnumerator() = raise (System.NotImplementedException())
//            member this.Name = raise (System.NotImplementedException())
//            member this.Parent = raise (System.NotImplementedException())
//            member this.PropertyByID
//                with get (propID) = raise (System.NotImplementedException())
//            member this.Save() = raise (System.NotImplementedException())
//            member this.Item
//                with get (index) = raise (System.NotImplementedException())


//    type Property() =
//        interface SolidEdgeFileProperties.Property with
//            member this.Item
//                with get (index) = raise (System.NotImplementedException())


//    let ExtractJdeFromCad location =

//        let prop = new ProperySets()
//        let prop = prop :> SolidEdgeFileProperties.PropertySets
//        prop.Open(location, true)

//        let pr = new Properties()
//        let pr = pr :> SolidEdgeFileProperties.Properties
//        let prr = pr.["Custom"]
//        prr
