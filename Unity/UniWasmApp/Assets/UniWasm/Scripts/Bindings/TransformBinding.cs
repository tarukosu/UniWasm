using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class TransformBinding : BindingBase
    {
        private Transform transform;

        public TransformBinding(Transform transform) : base()
        {
            this.transform = transform;
        }

        public override PredefinedImporter GenerateImporter()
        {
            var importer = new PredefinedImporter();
            importer.DefineFunction("transform_set_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.Vector3,
                     ValueType.Unit,
                     SetLocalPosition
                     ));

            importer.DefineFunction("transform_get_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Vector3,
                     GetLocalPosition
                     ));

            importer.DefineFunction("transform_set_local_rotation",
                 new DelegateFunctionDefinition(
                     ValueType.Quaternion,
                     ValueType.Unit,
                     SetLocalRotation
                     ));
            importer.DefineFunction("transform_get_local_rotation",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Quaternion,
                     GetLocalRotation
                     ));

            importer.DefineFunction("transform_set_local_scale",
                 new DelegateFunctionDefinition(
                     ValueType.Vector3,
                     ValueType.Unit,
                     SetLocalScale
                     ));

            importer.DefineFunction("transform_get_local_scale",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Vector3,
                     GetLocalScale
                     ));
            importer.DefineFunction("transform_get_local_scale_x",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Float,
                     _ => new object[] { transform.localScale.x }
                     ));
            importer.DefineFunction("transform_get_local_scale_y",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Float,
                     _ => new object[] { transform.localScale.y }
                     ));
            importer.DefineFunction("transform_get_local_scale_z",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Float,
                     _ => new object[] { transform.localScale.z }
                     ));
            return importer;
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

        private IReadOnlyList<object> SetLocalPosition(IReadOnlyList<object> arg)
        {
            if (arg.Count != 3)
            {
                return UniWasmUtils.Unit;
            }
            var x = (float)arg[0];
            var y = (float)arg[1];
            var z = (float)arg[2];

            transform.localPosition = new Vector3(x, y, z);
            return UniWasmUtils.Unit;
        }
        private IReadOnlyList<object> SetLocalRotation(IReadOnlyList<object> arg)
        {
            if (arg.Count != 4)
            {
                return UniWasmUtils.Unit;
            }
            var x = (float)arg[0];
            var y = (float)arg[1];
            var z = (float)arg[2];
            var w = (float)arg[3];

            transform.localRotation = new Quaternion(x, y, z, w);
            return UniWasmUtils.Unit;
        }


        private IReadOnlyList<object> GetLocalScale(IReadOnlyList<object> arg)
        {
            var scale = transform.localScale;

            return new object[]
            {
                scale.x, scale.y, scale.z
            };
        }

        private IReadOnlyList<object> SetLocalScale(IReadOnlyList<object> arg)
        {
            if (arg.Count != 3)
            {
                return UniWasmUtils.Unit;
            }
            var x = (float)arg[0];
            var y = (float)arg[1];
            var z = (float)arg[2];

            transform.localScale = new Vector3(x, y, z);
            return UniWasmUtils.Unit;
        }
    }
}
