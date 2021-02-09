namespace Replacer

open FSharp.Json

module TableConversion =

    type Table = Map<string, string>

    type ConversionChartList = Map<string, Table>

    let getEquivalentByTypeMaterial jdeNumber material =

        let json = System.IO.File.ReadAllText(Details.dataFileTables)

        let deserializedTableData = Json.deserialize<ConversionChartList> json

        let getTable (collectionsChart: ConversionChartList) jdeNum =
            collectionsChart.TryGetValue jdeNum

        let (Jde jdeNum) = jdeNumber
        let Success, table = getTable deserializedTableData jdeNum

        let partnumber  =
            if Success then table.[material]
            else ""

        match jdeNum with
            |"" -> failwithf """
MISSING DATA [!]
Number: %s has not entry in <table.json>. But you can update this file using the excel document
<J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\table.xlsx>
then you need to update the change by clicking on the macro <update.exe>.""" jdeNum
            | _ -> Chart.displayChart jdeNum table material

        let partnumber = Jde jdeNum
        partnumber
