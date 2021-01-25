namespace Tools

open FSharp.Json

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

        match item with
            | "","","" ->
                failwithf """Number %s is not an entry in <fasteners.json>, but you can add it yourself in the folder
                <J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\fasteners.json>
                """ jdeNumber
            | _,_,_ -> item

