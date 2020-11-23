// Learn more about F# at http://fsharp.org

open System
open System.IO
open FSharp.Data

[<Literal>]
let connectionString = "..."

[<EntryPoint>]
let main argv =
    let files = ["winequality-red.csv"; "winequality-white.csv"]

    let parseDecimal (f: string) =
        let (_, r) = Decimal.TryParse f
        r

    for file in files do
        let wineType = match file.Contains "red" with
                       | true -> "Red"
                       | false -> "White"

        let fileData = File.ReadAllLines file
        let data = fileData |> Array.skip 1 |> Array.map(fun line -> line.Split ';')

        use insertCmd = new SqlCommandProvider<"INSERT INTO WineData VALUES (@wineType, @fixedAcidity, @volatileAcidity, @citricAcid, @sugar, @chlorides, @freeSulfer, @totalSulfur, @density, @ph, @sulphates, @alcohol, @quality)", connectionString>(connectionString)

        for d in data do
            insertCmd.Execute(wineType, parseDecimal d.[0], parseDecimal d.[1], parseDecimal d.[2], 
                parseDecimal d.[3], parseDecimal d.[4], parseDecimal d.[5], parseDecimal d.[6], parseDecimal d.[7], parseDecimal d.[8], 
                parseDecimal d.[9], parseDecimal d.[10], parseDecimal d.[11]) |> ignore
        ()

    0 // return an integer exit code
