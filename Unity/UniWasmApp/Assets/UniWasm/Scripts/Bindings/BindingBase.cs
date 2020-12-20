using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public abstract class BindingBase
    {
        public ModuleInstance ModuleInstance { set; get; }
        public PredefinedImporter Importer { private set; get; }

        public BindingBase()
        {
            Importer = GenerateImporter();
        }

        public abstract PredefinedImporter GenerateImporter();
    }

    public class ArgumentParser
    {
        private IReadOnlyList<object> arg;
        private ModuleInstance moduleInstance;
        private int index;

        internal LinearMemory Memory
        {
            get
            {
                return moduleInstance.Memories[0];
            }
        }

        public ArgumentParser(IReadOnlyList<object> arg, ModuleInstance moduleInstance = null)
        {
            this.arg = arg;
            this.moduleInstance = moduleInstance;
        }

        public bool TryReadInt(out int value)
        {
            if (!TryReadObject(out var valueObject))
            {
                value = 0;
                return false;
            }

            try
            {
                value = (int)valueObject;
                return true;
            }
            catch (Exception e)
            {
                value = 0;
                Debug.LogWarning(e);
                return false;
            }
        }

        public bool TryReadString(out string value)
        {
            if (!TryReadInt(out var pointer) || !TryReadInt(out var length))
            {
                value = "";
                return false;
            }

            var memory = Memory;
            var data = new byte[length];

            try
            {
                for (var i = 0; i < length; i++)
                {
                    var index = (uint)(pointer + i);
                    data[i] = (byte)memory.Int8[index];
                }
                value = Encoding.UTF8.GetString(data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                value = "";
                return false;
            }
        }

        private bool TryReadObject(out object value)
        {
            //var nextIndex = index + 1;
            if (arg.Count <= index)
            {
                value = null;
                return false;
            }

            value = arg[index];
            index += 1;
            return true;
        }
    }
}
