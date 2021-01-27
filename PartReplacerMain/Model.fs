namespace Replacer

    module Model =

        type Jde = Jde of string
        type Rev = Rev of string
        type CadFileName = CadFileName of string

        type TCDetailsCad = Jde * Rev * CadFileName
