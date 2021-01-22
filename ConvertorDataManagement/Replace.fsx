open System.IO
open LoadPartsFromTeamcenter
open static System.Console
open Path = System.IO.Path
open Tools


namespace PartReplacer

let part occurence material =
    printfn("...")

    let partFullName = occ.OccurrenceFileName

    let cacheDirectory = Path.GetDirectoryName(partFullName) + Path.DirectorySeparatorChar

    let jdeOccurrence = Cache.GetJde(partFullName)

    let replacement = TableConversion.getEquivalentByTypeMaterial(jdeOccurrence, material)

    // Get details from jde number.
    let (jde, revision, filename) = Tools.Fasteners.getReplacementPartDetails(replacement)

    // logic here with parttern matching


    printfn("---")