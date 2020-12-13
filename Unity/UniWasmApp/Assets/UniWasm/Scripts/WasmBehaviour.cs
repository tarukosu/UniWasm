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
        private FunctionDefinition onTouchStartFunction;

        protected virtual void Update()
        {
            updateFunction?.Invoke(new object[0]);
        }

        public void InvokeOnTouchStart()
        {
            Debug.Log("invoke on touch start");
            onTouchStartFunction?.Invoke(new object[0]);
        }

        public void LoadWasm(string path, ContentsStore store = null)
        {
            var file = WasmFile.ReadBinary(path);
            LoadWasm(file, store);
        }

        public void LoadWasm(Stream stream, ContentsStore store = null)
        {
            var file = WasmFile.ReadBinary(stream);
            LoadWasm(file, store);
        }

        protected void LoadWasm(WasmFile file, ContentsStore store = null)
        {
            if (store == null)
            {
                store = new ContentsStore();
            }

            var importer = new PredefinedImporter();

            var wasiFunctions = new List<string>()
            {
                "proc_exit",
                "fd_write",
                "fd_prestat_get",
                "fd_prestat_dir_name",
                "environ_sizes_get",
                "environ_get",
                //"env.abort",
                "abort",
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

            var gameObjectBinding = new GameObjectBinding(transform, store);
            importer.IncludeDefinitions(gameObjectBinding.Importer);

            var transformBinding = new TransformBinding(transform, store);
            importer.IncludeDefinitions(transformBinding.Importer);

            var physicsBinding = new PhysicsBinding(transform, store);
            importer.IncludeDefinitions(physicsBinding.Importer);

            var timeBinding = new TimeBinding();
            importer.IncludeDefinitions(timeBinding.Importer);

            module = ModuleInstance.Instantiate(file, importer);

            var exportedFunctions = module.ExportedFunctions;
            exportedFunctions.TryGetValue("update", out updateFunction);
            exportedFunctions.TryGetValue("on_touch_start", out onTouchStartFunction);
        }
    }
}
