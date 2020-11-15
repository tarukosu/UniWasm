using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm.Interpret;

namespace UniWasm
{
    public class TimeBinding
    {
        public PredefinedImporter Importer { private set; get; }

        public TimeBinding()
        {
            InitImporter();
        }

        public void InitImporter()
        {
            var importer = new PredefinedImporter();

            importer.DefineFunction("time_get_time",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Float,
                     GetTime
                     ));

            importer.DefineFunction("time_get_delta_time",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Float,
                     GetDeltaTime
                     ));
            Importer = importer;
        }

        private IReadOnlyList<object> GetTime(IReadOnlyList<object> args)
        {
            return new object[]
            {
                Time.time
            };
        }
        private IReadOnlyList<object> GetDeltaTime(IReadOnlyList<object> args)
        {
            return new object[]
            {
                Time.deltaTime
            };
        }
    }
}
