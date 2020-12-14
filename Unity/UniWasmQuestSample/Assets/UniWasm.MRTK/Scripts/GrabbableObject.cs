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

    public void Initialize(
        WasmBehaviour wasm,
        MixedRealityInputAction grabAction,
        MixedRealityInputAction useAction)
    {
        this.wasm = wasm;

        var grabHandler = gameObject.AddComponent<CustomActionHandler>();
        grabHandler.InputAction = grabAction;
        grabHandler.SetFocusRequired(true);
        grabHandler.OnInputActionStarted += GrabHandler_OnInputActionStarted;

        var useHandler = gameObject.AddComponent<CustomActionHandler>();
        useHandler.InputAction = useAction;
        useHandler.SetFocusRequired(false);
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

        wasm.InvokeOnTouchStart();

    }

    private void GrabHandler_OnInputActionStarted(BaseInputEventData e)
    {
        var inputEventData = e as InputEventData;
        if (inputEventData == null)
        {
            return;
        }

        attachedHandedness = inputEventData.Handedness;

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

            // Interactions for a controller is the list of inputs that this controller exposes
            foreach (MixedRealityInteractionMapping inputMapping in controller.Interactions)
            {
                // 6DOF controllers support the "SpatialPointer" type (pointing direction)
                // or "GripPointer" type (direction of the 6DOF controller)
                if (inputMapping.InputType == DeviceInputType.SpatialPointer)
                {
                    Debug.Log("spatial pointer PositionData: " + inputMapping.PositionData);
                    Debug.Log("spatial pointer RotationData: " + inputMapping.RotationData);
                }

                if (inputMapping.InputType == DeviceInputType.SpatialGrip)
                {
                    Debug.Log("spatial grip PositionData: " + inputMapping.PositionData);
                    Debug.Log("spatial grip RotationData: " + inputMapping.RotationData);

                    var position = inputMapping.PositionData;
                    var rotation = inputMapping.RotationData;

                    transform.SetPositionAndRotation(position, rotation);
                }
            }
        }
    }
}
