open System
open Tools
open SolidEdgeFramework



[<STAThread>]
[<EntryPoint>]
let main argv =
    try
        User.displayDetails Details.author Details.version Details.update
        let response = Console.ReadLine().ToLower()

        match response with
        |response when response <> "y" ->
            printfn "You have exit the application."
            0

        |_ ->
            let application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true)
            let assemblyDocument = application.ActiveDocument :?> SolidEdgeAssembly.AssemblyDocument
            let selection = assemblyDocument.SelectSet
            let count = selection.Count
            Quantity.displaySelectionCount(selection.Count)
            match count with
            |0 ->
                printfn "Exit 0"
                0
            |_ ->
                Utilities.displayOptions |> ignore
                let material = Switcher.getUserChoice

                match material with
                |"" ->
                    Console.red   <| sprintf  "[!] Wrong pick! Try again but this time stick to the choice displayed."
                    printfn "" |> ignore
                    0

                |_ ->
                    for i in 1..count do
                        let mutable occ = selection.Item(i) :?> SolidEdgeAssembly.Occurrence
                        try
                            Exchanger.replace occ material |> ignore
                        with
                            | :? System.Exception as e -> printfn "%s" e.Message |> ignore
                    0
    finally
        System.Console.ReadKey() |> ignore
