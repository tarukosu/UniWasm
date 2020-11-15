using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Wasm;
using Wasm.Interpret;

namespace UniWasm
{
    public class WasmBehaviour : MonoBehaviour
    {
        private ModuleInstance module;
        private FunctionDefinition updateFunction;

        protected virtual void Update()
        {
            updateFunction?.Invoke(new object[0]);
        }

        public void LoadWasm(string path)
        {
            var file = WasmFile.ReadBinary(path);
            LoadWasm(file);
        }

        public void LoadWasm(Stream stream)
        {
            var file = WasmFile.ReadBinary(stream);
            LoadWasm(file);
        }

        protected void LoadWasm(WasmFile file)
        {
            var importer = new PredefinedImporter();

            var wasiFunctions = new List<string>()
            {
                "proc_exit",
                "fd_write",
                "fd_prestat_get",
                "fd_prestat_dir_name",
                "environ_sizes_get",
                "environ_get"
            };

            foreach (var wasiFunction in wasiFunctions)
            {
                importer.DefineFunction(wasiFunction,
                     new DelegateFunctionDefinition(
                         new WasmValueType[] { },
                         new WasmValueType[] { },
                         x => x
                         ));
            }

            var transformBinding = new TransformBinding(transform);
            importer.IncludeDefinitions(transformBinding.Importer);

            var timeBinding = new TimeBinding();
            importer.IncludeDefinitions(timeBinding.Importer);

            module = ModuleInstance.Instantiate(file, importer);

            var exportedFunctions = module.ExportedFunctions;
            exportedFunctions.TryGetValue("update", out updateFunction);
        }
    }
}
