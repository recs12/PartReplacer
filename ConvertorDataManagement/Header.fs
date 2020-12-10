namespace Helpers

module User =


    let displayDetails author version update =
        Console.cyan   <| sprintf   "PartReplacer :"
        printfn ""
        Console.cyan   <| sprintf "====================================================================="
        printfn ""
        Console.cyan   <| (sprintf " --author: %s --version: %s --last-update: %s" author version update)
        printfn ""
        Console.cyan   <| sprintf "---------------------------------------------------------------------"
        printfn ""
        Console.cyan   <|  @"Would you like to replace the fasteners in this assembly?"
        printfn ""
        Console.cyan   <|  @"Select the items you want to change and press y/[Y] to proceed:"
        printfn ""
        |>ignore