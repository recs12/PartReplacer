

//entries
let table = [("imperial ss-304", "24102605"); ("imperial ss-316", "147289");
 ("imperial zinc", "24100002"); ("metric ss-304", "24104927");
 ("metric ss-316", "182662"); ("metric zinc", "24101195")]
let materialList = [ "imperial zinc"; "metric zinc"; "imperial ss-304"; "metric ss-304"; "imperial ss-316"; "metric ss-316" ]

let displayChart jde chart materialList =

    let rec search dict key =
        match dict with
        | [] -> ""
        | (k, v) :: _ when k = key -> v
        | _ :: tl -> search tl key

    let signet jde key = if jde = key then ">" else " "
    let equality jde key = if jde = key then "==" else "<>"

    (*Display of the chart to user bellow.*)
    printfn "%s:" jde
    let wx = List.zip [1..6] materialList
    wx |> List.iter  (fun wx -> printfn "%4i|%s %-16s -> %-10s %s" (fst wx) (signet jde (search chart (snd wx))) (snd wx) (search chart (snd wx)) (equality jde (search chart (snd wx))))



displayChart "147289" table materialList