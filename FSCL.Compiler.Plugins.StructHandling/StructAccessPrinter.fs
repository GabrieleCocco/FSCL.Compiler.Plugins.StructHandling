﻿namespace FSCL.Compiler.StructHandling

open FSCL.Compiler
open FSCL.Compiler.FunctionPrettyPrinting
open Microsoft.FSharp.Quotations
open System.Collections.Generic
open System.Reflection

[<StepProcessor("FSCL_STRUCT_ACCESS_PRETTY_PRINTING_PROCESSOR", "FSCL_FUNCTION_PRETTY_PRINTING_STEP")>] 
type StructAccessPrinter() =                 
    interface FunctionBodyPrettyPrintingProcessor with
        member this.Process(expr, en) =
            let engine = en :?> FunctionPrettyPrintingStep
            match expr with
            | Patterns.PropertyGet(e, propertyInfo, args) ->
                if e.IsSome then
                    let t = e.Value.Type
                    if (t.IsValueType && (not t.IsPrimitive) && (not t.IsEnum)) then
                        Some(engine.Continue(e.Value) + "." + propertyInfo.Name)
                    else
                        None
                else 
                    None
            | Patterns.PropertySet(e, propertyInfo, args, body) ->                
                if e.IsSome then
                    let t = e.Value.Type
                    if (t.IsValueType && (not t.IsPrimitive) && (not t.IsEnum)) then
                        Some(engine.Continue(e.Value) + "." + propertyInfo.Name + " = " + engine.Continue(body))
                    else
                        None
                else 
                    None
            | _ ->
                None