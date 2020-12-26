using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class TransformBinding : BindingBase
    {
        public TransformBinding(Element element, ContentsStore store) : base(element, store)
        {
        }

        //private Transform transform;
        //private ContentsStore store;

        /*
        public TransformBinding(Transform transform, ContentsStore store) : base()
        {
            this.transform = transform;
            this.store = store;
        }
        */


        public override PredefinedImporter GenerateImporter()
        {
            var importer = new PredefinedImporter();

            // Local position
            foreach (Vector3ElementType axis in Enum.GetValues(typeof(Vector3ElementType)))
            {
                importer.DefineFunction($"transform_get_local_position_{axis}",
                     new DelegateFunctionDefinition(
                         ValueType.ObjectId,
                         ValueType.Float,
                         arg => GetTransformValue(arg, t => t.localPosition.GetSpecificValue(axis))
                         ));
            }

            importer.DefineFunction("transform_set_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndVector3,
                     ValueType.Unit,
                     SetLocalPosition
                     ));

            // World position
            foreach (Vector3ElementType axis in Enum.GetValues(typeof(Vector3ElementType)))
            {
                importer.DefineFunction($"transform_get_world_position_{axis}",
                     new DelegateFunctionDefinition(
                         ValueType.ObjectId,
                         ValueType.Float,
                         arg => GetTransformValue(arg, t => t.position.GetSpecificValue(axis))
                         ));
            }

            importer.DefineFunction("transform_set_world_position",
                 new DelegateFunctionDefinition(
                     ValueType.IdAndVector3,
                     ValueType.Unit,
                     SetWorldPosition
                     ));

            /*
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
            */

            /*
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
            */
            return importer;
        }

        private IReadOnlyList<object> InvalidFloatValue
        {
            get => ReturnValue.FromObject(float.NaN);
        }

        private IReadOnlyList<object> GetTransformValue(IReadOnlyList<object> arg, Func<Transform, float> func)
        {
            var parser = new ArgumentParser(arg);

            if (!TryGetElementWithArg(parser, store, out var element))
            {
                return InvalidFloatValue;
            }
            var value = func.Invoke(element.GameObject.transform);
            return ReturnValue.FromObject(value);
        }

/*
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
*/

        private IReadOnlyList<object> SetVector3(IReadOnlyList<object> arg, Action<Transform, Vector3> action)
        {
            var parser = new ArgumentParser(arg);
            if (!TryGetElementWithArg(parser, store, out var element))
            {
                return ReturnValue.Unit;
            }

            if (!parser.TryReadVector3(out var position))
            {
                return ReturnValue.Unit;
            }
            // element.GameObject.transform.localPosition = position;
            action?.Invoke(element.GameObject.transform, position);
            return ReturnValue.Unit;
        }

        private IReadOnlyList<object> SetLocalPosition(IReadOnlyList<object> arg)
        {
            return SetVector3(arg, (t, v) => t.localPosition = v);
        }

        private IReadOnlyList<object> SetWorldPosition(IReadOnlyList<object> arg)
        {
            return SetVector3(arg, (t, v) => t.position = v);
        }

        private IReadOnlyList<object> SetLocalRotation(IReadOnlyList<object> arg)
        {
            return UniWasmUtils.Unit;

            /*
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
            */
        }
        private IReadOnlyList<object> SetWorldRotation(IReadOnlyList<object> arg)
        {
            return UniWasmUtils.Unit;
            /*
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
            */
        }


        private IReadOnlyList<object> GetLocalScale(IReadOnlyList<object> arg)
        {
            return UniWasmUtils.Unit;

            /*
            var scale = transform.localScale;

            return new object[]
            {
                scale.x, scale.y, scale.z
            };
            */
        }

        private IReadOnlyList<object> SetLocalScale(IReadOnlyList<object> arg)
        {
            return UniWasmUtils.Unit;

            /*
            if (arg.Count != 3)
            {
                return UniWasmUtils.Unit;
            }
            var x = (float)arg[0];
            var y = (float)arg[1];
            var z = (float)arg[2];

            transform.localScale = new Vector3(x, y, z);
            return UniWasmUtils.Unit;
            */
        }

        /*
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
        */
    }
}
