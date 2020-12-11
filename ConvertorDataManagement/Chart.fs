namespace Tools

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
            Console._default   <| sprintf "%10i|" index
            Console.darkyellow <| sprintf "%s  " (arrow material category)
            Console._default   <| sprintf "%-16s" category
            Console._default   <| sprintf " -> "
            Console.darkyellow      <| sprintf " %-10s" (findIn chart category)
            Console._default   <| sprintf " %s"  (equality jde (findIn chart category))
            printfn ""

        Console._default <| sprintf "--- match"
        Console.green    <| sprintf " %8s" jde
        Console._default <| sprintf " with ---:"
        printfn ""

        printfn ""
        let zippedIndexAndCategories = List.zip [1..6] boltCategories
        displaylines zippedIndexAndCategories
        printfn ""
