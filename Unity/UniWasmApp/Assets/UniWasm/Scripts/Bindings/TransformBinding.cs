using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class TransformBinding
    {
        private Transform transform;
        public PredefinedImporter Importer { private set; get; }

        public TransformBinding(Transform transform)
        {
            this.transform = transform;
            InitImporter();
        }

        public void InitImporter()
        {
            var importer = new PredefinedImporter();
            importer.DefineFunction("transform_set_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.Vector3,
                     ValueType.Unit,
                     SetLocalPosition
                     ));

            importer.DefineFunction("transform_set_local_rotation",
                 new DelegateFunctionDefinition(
                     ValueType.Quaternion,
                     ValueType.Unit,
                     SetLocalPosition
                     ));

            importer.DefineFunction("transform_get_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Vector3,
                     GetLocalPosition
                     ));

            importer.DefineFunction("transform_get_local_rotation",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Quaternion,
                     GetLocalRotation
                     ));
            Importer = importer;
        }

        private IReadOnlyList<object> GetLocalRotation(IReadOnlyList<object> arg)
        {
            var rotation = transform.localRotation;
            return new object[]
            {
                rotation.x, rotation.y, rotation.z, rotation.w
            };
        }

        private IReadOnlyList<object> GetLocalPosition(IReadOnlyList<object> arg)
        {
            var position = transform.localPosition;
            return new object[]
            {
                position.x, position.y, position.z
            };
        }

        private IReadOnlyList<object> SetLocalPosition(IReadOnlyList<object> args)
        {
            if (args.Count != 3)
            {
                return UniWasmUtils.Unit;
            }
            var x = (float)args[0];
            var y = (float)args[1];
            var z = (float)args[2];

            transform.localPosition = new Vector3(x, y, z);
            return UniWasmUtils.Unit;
        }
        private IReadOnlyList<object> SetLocalQuaternion(IReadOnlyList<object> args)
        {
            if (args.Count != 4)
            {
                return UniWasmUtils.Unit;
            }
            var x = (float)args[0];
            var y = (float)args[1];
            var z = (float)args[2];
            var w = (float)args[3];

            transform.localRotation = new Quaternion(x, y, z, w);
            return UniWasmUtils.Unit;
        }
    }
}
