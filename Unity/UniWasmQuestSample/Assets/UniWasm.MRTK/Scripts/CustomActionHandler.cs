using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomActionHandler : BaseInputHandler, IMixedRealityInputActionHandler
{
    public MixedRealityInputAction InputAction { set; get; }
        = MixedRealityInputAction.None;

    public bool MarkEventsAsUsed { set; get; } = true;

    public Action<BaseInputEventData> OnInputActionStarted;
    public Action<BaseInputEventData> OnInputActionEnded;

    private void Awake()
    {
        // IsFocusRequired = true;
    }

    public void SetFocusRequired(bool required)
    {
        IsFocusRequired = required;
    }

    protected override void RegisterHandlers()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputActionHandler>(this);
    }

    protected override void UnregisterHandlers()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputActionHandler>(this);
    }

    void IMixedRealityInputActionHandler.OnActionEnded(BaseInputEventData eventData)
    {
    }

    void IMixedRealityInputActionHandler.OnActionStarted(BaseInputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == InputAction && !eventData.used)
        {
            OnInputActionStarted?.Invoke(eventData);
            if (MarkEventsAsUsed)
            {
                eventData.Use();
            }
        }
    }
}
