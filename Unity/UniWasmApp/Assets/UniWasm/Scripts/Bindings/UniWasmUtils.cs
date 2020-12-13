using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm;

namespace UniWasm
{
    public static class UniWasmUtils
    {
        public static IReadOnlyList<object> Unit = new object[0];

    }

    public static class ValueType
    {
        public static IReadOnlyList<WasmValueType> Unit = new WasmValueType[0];

        public static IReadOnlyList<WasmValueType> Float = new WasmValueType[]
        {
            WasmValueType.Float32,
        };

        public static IReadOnlyList<WasmValueType> Int = new WasmValueType[]
        {
            WasmValueType.Int32,
        };

        public static IReadOnlyList<WasmValueType> Vector3 = new WasmValueType[]
        {
            WasmValueType.Float32,
            WasmValueType.Float32,
            WasmValueType.Float32,
        };
        public static IReadOnlyList<WasmValueType> Quaternion = new WasmValueType[]
        {
            WasmValueType.Float32,
            WasmValueType.Float32,
            WasmValueType.Float32,
            WasmValueType.Float32,
        };
    }
}
