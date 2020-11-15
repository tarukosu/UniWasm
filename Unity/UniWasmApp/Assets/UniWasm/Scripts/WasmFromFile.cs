using UnityEngine;

namespace UniWasm
{
    public class WasmFromFile : WasmBehaviour
    {
        [SerializeField]
        private string filePath;

        private void Awake()
        {
            LoadWasm(filePath);
        }
    }
}
