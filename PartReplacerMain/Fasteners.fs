namespace Tools

open FSharp.Json
open Model

module Fasteners =

    type FastenerDetails = {
            JdeNumber: string
            Revision: string
            Filename: string
    }

    type ItemCollection = FastenerDetails list

    let getReplacementPartDetails jde =

        let (Jde jdeNumber) = jde

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

        //exception MissingDataInFastenersJson as string

        match item with
            | "","","" ->
                failwithf """
MISSING DATA [!]
Number: %s has not entry in <fasteners.json>. But you can update this file using the excel document
<J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\fasteners.xlsx>
then you need to update the change by clicking on the macro <update.exe>.

                """ jdeNumber
            | _,_,_ -> item

