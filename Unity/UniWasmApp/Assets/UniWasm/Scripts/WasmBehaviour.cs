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
        private FunctionDefinition onUseFunction;

        protected virtual void Update()
        {
            updateFunction?.Invoke(new object[0]);
        }

        public void InvokeOnUse()
        {
            onUseFunction?.Invoke(new object[0]);
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

            var element = new Element()
            {
                GameObject = gameObject
            };

            var argsBinding = new ArgsBinding(element, store);
            importer.IncludeDefinitions(argsBinding.Importer);

            var debugBinding = new DebugBinding(element, store);
            importer.IncludeDefinitions(debugBinding.Importer);

            var gameObjectBinding = new GameObjectBinding(element, store);
            importer.IncludeDefinitions(gameObjectBinding.Importer);

            var transformBinding = new TransformBinding(element, store);
            importer.IncludeDefinitions(transformBinding.Importer);

            var physicsBinding = new PhysicsBinding(element, store);
            importer.IncludeDefinitions(physicsBinding.Importer);

            var timeBinding = new TimeBinding(element, store);
            importer.IncludeDefinitions(timeBinding.Importer);

            module = ModuleInstance.Instantiate(file, importer);

            argsBinding.ModuleInstance = module;
            gameObjectBinding.ModuleInstance = module;
            debugBinding.ModuleInstance = module;

            var exportedFunctions = module.ExportedFunctions;
            exportedFunctions.TryGetValue("update", out updateFunction);
            exportedFunctions.TryGetValue("on_touch_start", out onTouchStartFunction);
            exportedFunctions.TryGetValue("on_use", out onUseFunction);
        }
    }
}
