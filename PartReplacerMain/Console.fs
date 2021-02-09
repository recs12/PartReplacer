namespace Replacer

module Console =

    open System

    let log =
        let lockObj = obj()
        fun color s ->
            lock lockObj (fun _ ->
                Console.ForegroundColor <- color
                printf "%s" s
                Console.ResetColor())

    let magenta    = log ConsoleColor.Magenta
    let darkyellow = log ConsoleColor.DarkYellow
    let green      = log ConsoleColor.Green
    let cyan       = log ConsoleColor.Cyan
    let yellow     = log ConsoleColor.Yellow
    let red        = log ConsoleColor.Red
    let _default   = log ConsoleColor.White


    (*Implement this function to decluster the coloring in the tool*)

    let printInWhite message =
        log ConsoleColor.White   <| sprintf message

    let printfnInWhite message  =
        log ConsoleColor.White   <| sprintf message
        printfn ""

    let printInRed message =
        log ConsoleColor.White   <| sprintf message
