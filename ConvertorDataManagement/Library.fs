namespace Helpers

open FSharp.Json

module TableConversion =

    type Table = Map<string, string>

    type ConversionChartList = Map<string, Table>

    let getEquivalentByTypeMaterial jdeNumber (material) =

        let json = System.IO.File.ReadAllText(Details.dataFileTables)

        let deserializedTableData = Json.deserialize<ConversionChartList> json

        let getTable (collectionsChart: ConversionChartList) jdeNum =
            collectionsChart.TryGetValue jdeNum

        let Success, table = getTable deserializedTableData jdeNumber

        let partnumber =
            if (Success && material <> "?") then table.[material]
            else ""

        Chart.displayChart jdeNumber table material

        partnumber