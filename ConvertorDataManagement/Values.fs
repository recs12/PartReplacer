
module Details

    let version : string = "0.0.3"
    let author : string = "recs"
    let update : string = "2020-11-19"

    let BoltCategories : list<string> = [ "imperial zinc"; "metric zinc"; "imperial ss-304"; "metric ss-304"; "imperial ss-316"; "metric ss-316" ]

    let dataFileFasteners : string =
        @"J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\fasteners.json"

    let dataFileTables =
        @"J:\PTCR\Users\RECS\Macros\ReplacerFasteners\dataFastenersJson\table.json"