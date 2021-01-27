namespace Replacer

open PartReplacer
open System.IO
open Model
open Teamcenter.Replacer

module Exchanger =

    let replace occ material =

        let oc = occ :> SolidEdgeAssembly.Occurrence

        let partFullName : string = oc.OccurrenceFileName

        printfn "..."

        let cacheDirectory : string =  System.IO.Path.GetDirectoryName(partFullName) + @"\"

        let jdeOccurrence : string = Cache.GetJde(partFullName)

        let occurrence : Jde = Jde jdeOccurrence

        let replacement : Jde = TableConversion.getEquivalentByTypeMaterial occurrence material

        let (jde, revision, filename) = Replacer.Fasteners.getReplacementPartDetails(replacement)

        let loadFromTC (jde: string) (revision: string) (filename: string) cacheDirectory =
            match jde, revision, filename with
                | "", "", "" -> failwithf "part not available in <fasteners.json>"
                | _, _, _ -> Tc.LoadPartToCache(jde, revision, filename, cacheDirectory)

        loadFromTC jde revision filename |> ignore

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
