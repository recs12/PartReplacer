namespace Tools

open Teamcenter.PartReplacer
open System.IO
open PartReplacer
open Model

module Exchanger =

    let replace occ material =

        let oc = occ :> SolidEdgeAssembly.Occurrence

        let partFullName = oc.OccurrenceFileName

        printfn "..."

        let cacheDirectory =  System.IO.Path.GetDirectoryName(partFullName) + @"\"

        let jdeOccurrence = Cache.GetJde(partFullName)

        let occurrence : Jde = Jde jdeOccurrence

        let replacement = TableConversion.getEquivalentByTypeMaterial occurrence material

        let (jde, revision, filename) = Tools.Fasteners.getReplacementPartDetails(replacement)

        let load (jde: string) (revision: string) (filename: string) cacheDirectory =
            match jde, revision, filename with
                | "", "", "" ->
                    failwithf "part not available in <fasteners.json>"
                | _, _, _ ->
                    Tc.LoadPartToCache(jde, revision, filename, cacheDirectory)

        load jde revision filename |> ignore

        let newPartPath = Path.Combine(cacheDirectory, filename)

        match File.Exists(newPartPath) with
            | true ->
                oc.Replace(newPartPath, true)
                let (Jde jdeNum ) = replacement
                printfn "[+] Replaced: %s -> %s" jdeOccurrence jdeNum
            | false ->
                printfn "[!] Replacement part was not loaded in your cache, maybe check if you are really connected to Teamcenter."

        printfn "---" |> ignore
        0
