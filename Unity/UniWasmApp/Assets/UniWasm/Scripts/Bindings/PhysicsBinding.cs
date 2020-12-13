﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class PhysicsBinding : BindingBase
    {
        private Transform transform;
        private ContentsStore store;

        private Dictionary<string, Rigidbody> rigidbodyDictionary = new Dictionary<string, Rigidbody>();

        public PhysicsBinding(Transform transform, ContentsStore store) : base()
        {
            this.transform = transform;
            this.store = store;
        }

        public override PredefinedImporter GenerateImporter()
        {
            var importer = new PredefinedImporter();

            /*
            importer.DefineFunction("physics_set_local_velocity",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndVector3,
                     ValueType.Unit,
                     SetLocalVelocity
                     ));
            */
            importer.DefineFunction("physics_set_world_velocity",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndVector3,
                     ValueType.Unit,
                     SetWorldVelocity
                     ));
            return importer;
        }

        /*
        private IReadOnlyList<object> SetLocalVelocity(IReadOnlyList<object> arg)
        {
            if (arg.Count != 4)
            {
                return UniWasmUtils.Unit;
            }
            var objectId = (int)arg[0];

            var x = (float)arg[1];
            var y = (float)arg[2];
            var z = (float)arg[3];
            var velocity = new Vector3(x, y, z);

            Debug.Log(objectId);
            var key = objectId.ToString();

            if (!TryGetRigidbody(key, out var rigidbody))
            {
                return UniWasmUtils.Unit;
            }

            rigidbody.velocity = velocity;
            return UniWasmUtils.Unit;
        }
*/
        private IReadOnlyList<object> SetWorldVelocity(IReadOnlyList<object> arg)
        {
            if (arg.Count != 4)
            {
                return UniWasmUtils.Unit;
            }
            var objectId = (int)arg[0];

            var x = (float)arg[1];
            var y = (float)arg[2];
            var z = (float)arg[3];
            var velocity = new Vector3(x, y, z);

            Debug.Log(objectId);
            var key = objectId.ToString();

            if (!TryGetRigidbody(key, out var rigidbody))
            {
                return UniWasmUtils.Unit;
            }

            rigidbody.velocity = velocity;
            return UniWasmUtils.Unit;
        }


        private bool TryGetRigidbody(string key, out Rigidbody rigidbody)
        {
            if (!rigidbodyDictionary.TryGetValue(key, out rigidbody))
            {
                if (!store.Objects.TryGetValue(key, out var gameObject))
                {
                    return false;
                }

                rigidbody = gameObject.GetComponent<Rigidbody>();
                if (rigidbody == null)
                {
                    rigidbody = gameObject.AddComponent<Rigidbody>();
                    rigidbody.useGravity = false;
                }

                rigidbodyDictionary[key] = rigidbody;
            }

            return true;
        }
    }
}
