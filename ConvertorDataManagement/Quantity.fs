namespace Helpers

module Quantity =

    let displaySelectionCount count =
        Console.cyan <| sprintf @"Number of items selected: "
        Console.red  <| sprintf "** %i **" count
        printfn ""
