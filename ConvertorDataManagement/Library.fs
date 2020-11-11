
module Utilities

    let version : string = "0.0.1"
    let author : string = "recs"
    let update : string = "2020-11-04"

    let tablePath : string = @"J:\PTCR\Users\RECS\Macros\Replacer\dataFastenersJson\table.json"
    let fastenersPath : string = @"J:\PTCR\Users\RECS\Macros\Replacer\dataFastenersJson\fasteners.json"

    let displayDetails author version update =
        printfn "PartReplacer  --author:%s --version:%s --last-update :%s" author version update


    let zip s1 s2 = List.zip s1 s2 |> List.ofSeq

    let printOptionLine x = printfn """[%i] %s -""" (fst x) (snd x)

    let displayOptions =
        let material : list<string> = [ "imperial zinc"; "metric zinc"; "imperial ss-304"; "metric ss-304"; "imperial ss-316"; "metric ss-316" ]
        let indexer = [1 .. 6]
        printfn ""
        let sx = zip indexer material
        sx
        |> List.iter printOptionLine
        printfn ""
        0

