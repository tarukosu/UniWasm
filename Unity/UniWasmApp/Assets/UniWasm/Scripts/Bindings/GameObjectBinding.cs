using System.Collections;
using System.Collections.Generic;
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
            importer.DefineFunction("object_spawn_object",
                 new DelegateFunctionDefinition(
                     ValueType.Int,
                     ValueType.Int,
                     SpawnObject
                     ));
            return importer;
        }

        private IReadOnlyList<object> SpawnObject(IReadOnlyList<object> arg)
        {
            if (arg.Count != 1)
            {
                return UniWasmUtils.Unit;
            }

            var resourceId = arg[0].ToString();
            if(store.ResourceObjects.TryGetValue(resourceId, out var resourceObject))
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
    }
}
