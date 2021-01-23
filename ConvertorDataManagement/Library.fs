namespace Tools

open FSharp.Json

module TableConversion =
    open Chart

    type Table = Map<string, string>

    type ConversionChartList = Map<string, Table>

    let getEquivalentByTypeMaterial jdeNumber material =

        let json = System.IO.File.ReadAllText(Details.dataFileTables)

        let deserializedTableData = Json.deserialize<ConversionChartList> json

        let getTable (collectionsChart: ConversionChartList) jdeNum =
            collectionsChart.TryGetValue jdeNum

        let Success, table = getTable deserializedTableData jdeNumber

        let partnumber  =
            if Success then table.[material]
            else ""

        match partnumber with
            |"" -> failwithf """Number %s is not an entry in <table.json>, but you can add it yourself in the folder
                             <J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\table.json>
                             """ jdeNumber
            |_ -> Chart.displayChart jdeNumber table material

        partnumber
