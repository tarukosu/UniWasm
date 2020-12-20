using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class TransformBinding : BindingBase
    {
        private Transform transform;
        private ContentsStore store;

        public TransformBinding(Transform transform, ContentsStore store) : base()
        {
            this.transform = transform;
            this.store = store;
        }

        public override PredefinedImporter GenerateImporter()
        {
            var importer = new PredefinedImporter();
            importer.DefineFunction("transform_set_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndVector3,
                     ValueType.Unit,
                     SetLocalPosition
                     ));

            importer.DefineFunction("transform_set_world_position",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndVector3,
                     ValueType.Unit,
                     SetWorldPosition
                     ));
            /*
            importer.DefineFunction("transform_get_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Vector3,
                     GetLocalPosition
                     ));
            */

            // get local position
            importer.DefineFunction("transform_get_local_position_x",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localPosition.x }
                     ));

            importer.DefineFunction("transform_get_local_position_y",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localPosition.y }
                     ));
            importer.DefineFunction("transform_get_local_position_z",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localPosition.z }
                     ));

            importer.DefineFunction("transform_get_world_position_x",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.position.x }
                     ));

            importer.DefineFunction("transform_get_world_position_y",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.position.y }
                     ));
            importer.DefineFunction("transform_get_world_position_z",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.position.z }
                     ));

            // temporary function 
            importer.DefineFunction("transform_get_world_forward_x",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.forward.x }
                     ));
            importer.DefineFunction("transform_get_world_forward_y",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.forward.y }
                     ));
            importer.DefineFunction("transform_get_world_forward_z",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.forward.z }
                     ));
            // temporary function end

            importer.DefineFunction("transform_get_local_position_y",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localPosition.y }
                     ));
            importer.DefineFunction("transform_get_local_position_z",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localPosition.z }
                     ));
            // local rotation
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

            // world rotation
            importer.DefineFunction("transform_set_world_rotation",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndQuaternion,
                     ValueType.Unit,
                     SetWorldRotation
                     ));
            importer.DefineFunction("transform_get_world_rotation_x",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.rotation.x }
                     ));
            importer.DefineFunction("transform_get_world_rotation_y",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.rotation.y }
                     ));
            importer.DefineFunction("transform_get_world_rotation_z",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.rotation.z }
                     ));
            importer.DefineFunction("transform_get_world_rotation_w",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.rotation.w }
                     ));

            importer.DefineFunction("transform_set_local_scale",
                 new DelegateFunctionDefinition(
                     ValueType.Vector3,
                     ValueType.Unit,
                     SetLocalScale
                     ));
            /*
            importer.DefineFunction("transform_get_local_scale",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Vector3,
                     GetLocalScale
                     ));
            */

            importer.DefineFunction("transform_get_local_scale_x",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localScale.x }
                     ));
            importer.DefineFunction("transform_get_local_scale_y",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
                     ValueType.Float,
                     _ => new object[] { transform.localScale.y }
                     ));
            importer.DefineFunction("transform_get_local_scale_z",
                 new DelegateFunctionDefinition(
                     ValueType.ObjectId,
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
            if (arg.Count != 4)
            {
                return UniWasmUtils.Unit;
            }
            var objectId = (int)arg[0];

            var x = (float)arg[1];
            var y = (float)arg[2];
            var z = (float)arg[3];

            if (!TryGetTransform(objectId, out var transform))
            {
                return UniWasmUtils.Unit;
            }

            transform.localPosition = new Vector3(x, y, z);
            return UniWasmUtils.Unit;
        }
        private IReadOnlyList<object> SetWorldPosition(IReadOnlyList<object> arg)
        {
            if (arg.Count != 4)
            {
                return UniWasmUtils.Unit;
            }
            var objectId = (int)arg[0];

            var x = (float)arg[1];
            var y = (float)arg[2];
            var z = (float)arg[3];

            if (!TryGetTransform(objectId, out var transform))
            {
                return UniWasmUtils.Unit;
            }

            transform.position = new Vector3(x, y, z);
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
        private IReadOnlyList<object> SetWorldRotation(IReadOnlyList<object> arg)
        {
            if (arg.Count != 5)
            {
                return UniWasmUtils.Unit;
            }

            var objectId = (int)arg[0];

            var x = (float)arg[1];
            var y = (float)arg[2];
            var z = (float)arg[3];
            var w = (float)arg[4];

            if (!TryGetTransform(objectId, out var transform))
            {
                return UniWasmUtils.Unit;
            }
            transform.rotation = new Quaternion(x, y, z, w);
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

        private bool TryGetTransform(int objectId, out Transform transform)
        {
            var key = objectId.ToString();
            if (!store.Objects.TryGetValue(key, out var gameObject))
            {
                transform = default;
                return false;
            }

            transform = gameObject.transform;
            return true;
        }
    }
}
