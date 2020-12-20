using System.Collections;
using System.Collections.Generic;
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
}
