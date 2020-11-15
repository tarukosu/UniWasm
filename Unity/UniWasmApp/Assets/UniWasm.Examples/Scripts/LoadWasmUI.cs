using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniWasm;

public class LoadWasmUI : MonoBehaviour
{
    [SerializeField]
    WasmFromUrl wasmFromUrl;

    [SerializeField]
    private Button button;

    [SerializeField]
    private InputField inputField;

    void Awake()
    {
        button.onClick.AddListener(() =>
        {
            var url = inputField.text;
            _ = wasmFromUrl.LoadWasmFromUrl(url);
        });
    }
}
