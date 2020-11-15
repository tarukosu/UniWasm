using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm;
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
            importer.DefineFunction("set_local_position",
                 new DelegateFunctionDefinition(
                     ValueType.Vector3,
                     ValueType.Unit,
                     SetLocalPosition
                     ));

            Importer = importer;
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
    }
}
