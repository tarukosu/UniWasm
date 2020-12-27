using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class ArgsBinding : BindingBase
    {
        public ArgsBinding(Element element, ContentsStore store) : base(element, store)
        {
        }

        public override PredefinedImporter GenerateImporter()
        {
            var importer = new PredefinedImporter();

            importer.DefineFunction("args_get",
                 new DelegateFunctionDefinition(
                     ValueType.PointerAndPointer,
                     ValueType.Short,
                     ArgsGet
                     ));
            importer.DefineFunction("args_sizes_get",
                 new DelegateFunctionDefinition(
                     ValueType.PointerAndPointer,
                     ValueType.Short,
                     ArgsSizesGet
                     ));
            return importer;
        }

        private IReadOnlyList<object> ErrorResult
        {
            get => ReturnValue.FromObject(0);
        }

        private IReadOnlyList<object> SuccessResult
        {
            get => ReturnValue.FromObject(0);
        }

        private IReadOnlyList<object> ArgsGet(IReadOnlyList<object> arg)
        {
            var memory = ModuleInstance.Memories[0];
            var memory8 = memory.Int8;
            var memory32 = memory.Int32;

            var parser = new ArgumentParser(arg, ModuleInstance);
            if (!parser.TryReadUInt(out uint argvOffset))
            {
                return ErrorResult;
            }
            if (!parser.TryReadUInt(out uint argvBufferOffset))
            {
                return ErrorResult;
            }

            for (var i = 0; i < 2; i++)
            {
                // value = InterpretAsUint(intValue);
                memory32[argvOffset] = ArgumentParser.InterpretAsInt(argvBufferOffset);
                //argvOffset += 1;
                argvOffset += 4;


                memory8[argvBufferOffset] = 65;
                memory8[argvBufferOffset + 1] = 66;
                memory8[argvBufferOffset + 2] = 0;
                argvBufferOffset += 3;
            }

            return SuccessResult;
        }

        private IReadOnlyList<object> ArgsSizesGet(IReadOnlyList<object> arg)
        {
            var memory = ModuleInstance.Memories[0];
            var memory8 = memory.Int8;
            var memory32 = memory.Int32;

            var parser = new ArgumentParser(arg, ModuleInstance);
            if (!parser.TryReadUInt(out uint argvOffset))
            {
                return ErrorResult;
            }
            if (!parser.TryReadUInt(out uint argvBufferOffset))
            {
                return ErrorResult;
            }

            var argc = 2;
            memory32[argvOffset] = argc;
            memory32[argvBufferOffset] = 6;

            return SuccessResult;
        }
    }
}
