open System
open Replacer

[<STAThread>]
[<EntryPoint>]
let main argv =
    try
        printfn    "PartReplacer :"
        printfn  "====================================================================="
        printfn  " --author: %s --version: %s --last-update: %s" Details.author Details.version Details.update
        printfn  "---------------------------------------------------------------------"
        printfn  @"Would you like to replace the fasteners in this assembly?"
        printfn  @"Select the items you want to change and press y/[Y] to proceed:"

        let response = Console.ReadLine().ToLower()

        match response with
        | _ when response <> "y" ->
            printfn "You have exit the application."
            0

        | _ ->
            let application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true)

            let assemblyDocument = application.ActiveDocument :?> SolidEdgeAssembly.AssemblyDocument

            let selection = assemblyDocument.SelectSet

            let count = selection.Count

            printfn "Number of items selected: ** %i **" count

            match count with
            | 0 ->
                printfn """[!] No active selection: Select some items before running the macro again."""
                0
            | _ ->
                Utilities.displayChoices |> ignore

                let material = Switcher.getUserChoice

                match material with
                | "" ->
                    printfn  "[!] Wrong pick! Try again but this time stick to the choice displayed."
                    0

                | _ ->

                        for i in 1..count do
                            printfn "%i" i
                            let mutable occ = selection.Item(i)
                            match occ with
                            | :? SolidEdgeAssembly.Occurrence
                                ->
                                let mutable occ = selection.Item(i) :?> SolidEdgeAssembly.Occurrence
                                try
                                    Exchanger.replace occ material |> ignore
                                with
                                | _ as e
                                    -> printfn "[!] %s" e.Message |> ignore

                            | :? SolidEdgeAssembly.SubOccurrence
                                ->
                                let mutable occ = selection.Item(i) :?> SolidEdgeAssembly.SubOccurrence
                                printfn "[!] %A : SubOccurrence : You can only select an item within the editable assembly." occ.Name
                            | _
                                -> printfn "[!] invalid selection."
                        0

    finally
        System.Console.ReadKey() |> ignore