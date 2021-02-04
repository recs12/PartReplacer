namespace Replacer

open PartReplacer
open System.IO
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

        let partDetails : DetailsCad = Replacer.Fasteners.getReplacementPartDetails(replacement)


        let loadFromTC partdetails =
            let (Jde jde) = partDetails.Jde
            let (Rev rev) = partDetails.Rev
            let (CadFileName filename) = partDetails.CadFileName
            match jde, rev, filename with
                | "", "", "" -> failwithf "part not available in <fasteners.json>"
                | _, _, _ -> Tc.LoadPartToCache(jde, rev, filename, cacheDirectory)

        (*Load the replacement part in the solidedge user cache*)
        loadFromTC partDetails |> ignore

        let getPathNewCadFile cad directory =
            let (CadFileName filename) = cad.CadFileName
            let newPartPath = Path.Combine(directory, filename)
            newPartPath

        let newPartPath = getPathNewCadFile partDetails cacheDirectory

        match File.Exists(newPartPath) with
            | true ->
                oc.Replace(newPartPath, true)
                let (Jde jdeNum ) = replacement
                printfn "[+] Replaced: %s -> %s" jdeOccurrence jdeNum
            | false ->
                printfn "[!] Replacement part was not loaded in your cache, maybe check if you are really connected to Teamcenter."

        printfn "---" |> ignore
        0
