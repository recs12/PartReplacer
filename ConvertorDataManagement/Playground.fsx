

//entries
let table = [("imperial ss-304", "24102605"); ("imperial ss-316", "147289");
 ("imperial zinc", "24100002"); ("metric ss-304", "24104927");
 ("metric ss-316", "182662"); ("metric zinc", "24101195")]
let materialList = [ "imperial zinc"; "metric zinc"; "imperial ss-304"; "metric ss-304"; "imperial ss-316"; "metric ss-316" ]

let displayChart jde chart =

    let boltCategories = materialList

    let rec search dict key =
        match dict with
        | [] -> ""
        | (k, v) :: _ when k = key -> v
        | _ :: tl -> search tl key

    let signet jde key = if jde = key then ">" else " "
    let equality jde key = if jde = key then "==" else "<>"

    let displaylines line =
        for index, table in line do
        printfn "%4i|%s %-16s -> %-10s %s" index (signet jde (search chart table)) table (search chart table) (equality jde (search chart table))

    (*Display of the chart to user.*)

    printfn "match %-8s with" jde
    let zipped = List.zip [1..6] boltCategories
    displaylines zipped


displayChart "147289" table