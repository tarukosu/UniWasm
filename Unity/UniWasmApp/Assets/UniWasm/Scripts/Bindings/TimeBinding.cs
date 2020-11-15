using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wasm;
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

            importer.DefineFunction("get_time",
                 new DelegateFunctionDefinition(
                     ValueType.Unit,
                     ValueType.Float,
                     GetTime
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
    }
}
