using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class GameObjectBinding : BindingBase
    {
        private Transform transform;
        private ContentsStore store;

        public GameObjectBinding(Transform transform, ContentsStore store) : base()
        {
            this.transform = transform;
            this.store = store;
        }

        public override PredefinedImporter GenerateImporter()
        {
            var importer = new PredefinedImporter();
            importer.DefineFunction("element_spawn_object",
                 new DelegateFunctionDefinition(
                     ValueType.Int,
                     ValueType.Int,
                     SpawnObject
                     ));

            importer.DefineFunction("element_get_resource_index_by_id",
                 new DelegateFunctionDefinition(
                     ValueType.String,
                     ValueType.Int64,
                     GetResourceIndexById
                     ));
            return importer;
        }

        private IReadOnlyList<object> GetResourceIndexById(IReadOnlyList<object> arg)
        {
            var ptr = (int)arg[0];
            var length = (int)arg[1];

            var memory = ModuleInstance.Memories[0];
            var text = ReadString(memory, ptr, length);
            Debug.Log(text);

            return new object[]{
                (long)0
            };
        }

        private IReadOnlyList<object> SpawnObject(IReadOnlyList<object> arg)
        {
            if (arg.Count != 1)
            {
                return UniWasmUtils.Unit;
            }

            var resourceId = arg[0].ToString();
            if (store.ResourceObjects.TryGetValue(resourceId, out var resourceObject))
            {
                var root = store.RootTransform;
                var go = Object.Instantiate(resourceObject);
                go.transform.SetParent(root, false);
                var id = store.RegisterObject(go);
                go.name = id.ToString();
                Debug.Log($"Spawn Object, ObjectId: {id}");
                return new object[]
                {
                    id
                };
            }

            return UniWasmUtils.Unit;
        }

        internal static string ReadString(LinearMemory memory, int pointer, int length)
        {
            var data = new byte[length];
            for (var i = 0; i < length; i++)
            {
                var index = (uint)(pointer + i);
                data[i] = (byte)memory.Int8[index];
            }
            var text = Encoding.UTF8.GetString(data);
            return text;
        }
    }
}
