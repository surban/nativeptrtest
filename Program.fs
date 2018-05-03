// Learn more about F# at http://fsharp.org

open System
open FSharp.NativeInterop
open System.IO.Compression
open System.Reflection

open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Attributes.Jobs
open BenchmarkDotNet.Running
open BenchmarkDotNet.Order
open BenchmarkDotNet.Columns
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Horology
open BenchmarkDotNet.Reports
open BenchmarkDotNet.Exporters
open BenchmarkDotNet.Exporters.Csv


#nowarn "9"

[<DisassemblyDiagnoser(printIL=true, printAsm=true, printSource=true, printPrologAndEpilog=true)>]
[<RyuJitX64Job>]
type Benchmark () =

    [<Benchmark>]
    member __.NativeGet () =
        let a = NativePtr.stackalloc<float> 100       
        NativePtr.set a 10 44.0
        let a10 = NativePtr.get a 10
        a10 + 100.0

    [<Benchmark>]
    member __.NativeOffset () =
        let a = NativePtr.stackalloc<float> 100          
        NativePtr.set a 10 55.0
        let a10 = (a |> NativePtr.toNativeInt) + 20n |> NativePtr.ofNativeInt<float> |> NativePtr.read
        a10 + 200.0


[<EntryPoint>]
let main argv =
    let switcher = BenchmarkSwitcher (Assembly.GetExecutingAssembly())
    switcher.RunAll () |> ignore
    0