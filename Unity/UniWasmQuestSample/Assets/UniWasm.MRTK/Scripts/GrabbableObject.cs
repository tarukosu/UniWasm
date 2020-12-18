using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniWasm;

public class GrabbableObject : MonoBehaviour
{
    Handedness attachedHandedness = Handedness.None;
    WasmBehaviour wasm;

    private CustomActionHandler grabHandler;
    private CustomActionHandler useHandler;

    public void Initialize(
        WasmBehaviour wasm,
        MixedRealityInputAction grabAction,
        MixedRealityInputAction useAction)
    {
        this.wasm = wasm;

        grabHandler = gameObject.AddComponent<CustomActionHandler>();
        grabHandler.InputAction = grabAction;
        grabHandler.SetFocusRequired(true);
        grabHandler.OnInputActionStarted += GrabHandler_OnInputActionStarted;

        useHandler = gameObject.AddComponent<CustomActionHandler>();
        useHandler.InputAction = useAction;
        // useHandler.SetFocusRequired(false);
        useHandler.OnInputActionStarted += UseHandler_OnInputActionStarted;
    }

    private void UseHandler_OnInputActionStarted(BaseInputEventData e)
    {
        var inputEventData = e as InputEventData;
        if (inputEventData == null)
        {
            return;
        }

        if (attachedHandedness != inputEventData.Handedness)
        {
            return;
        }

        wasm.InvokeOnUse();

    }

    private void GrabHandler_OnInputActionStarted(BaseInputEventData e)
    {
        var inputEventData = e as InputEventData;
        if (inputEventData == null)
        {
            return;
        }

        attachedHandedness = inputEventData.Handedness;

        useHandler.SetFocusRequired(false);
        Debug.Log(e.InputSource);
    }

    private void Update()
    {
        if (attachedHandedness == Handedness.None)
        {
            return;
        }

        foreach (var controller in CoreServices.InputSystem.DetectedControllers)
        {
            if (controller.ControllerHandedness != attachedHandedness)
            {
                continue;
            }

            var inputType = DeviceInputType.SpatialPointer;
            foreach (MixedRealityInteractionMapping inputMapping in controller.Interactions)
            {
                if (inputMapping.InputType == inputType)
                {
                    var position = inputMapping.PositionData;
                    var rotation = inputMapping.RotationData;

                    transform.SetPositionAndRotation(position, rotation);
                }
            }
        }
    }
}
