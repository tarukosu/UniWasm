using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace UniWasm.MRTK
{
    public class LoaderForMRTK : SemlLoader
    {
        [SerializeField]
        private MixedRealityInputAction grabAction;

        [SerializeField]
        private MixedRealityInputAction useAction;

        protected override WasmBehaviour AttatchScript(string path, XmlNode node, Transform parent)
        {
            var wasm = base.AttatchScript(path, node, parent);
            if (wasm != null)
            {
                var grabbableObject = wasm.gameObject.AddComponent<GrabbableObject>();
                grabbableObject.Initialize(wasm, grabAction, useAction);

                /*
                grabbableObject.InputAction = grabAction;
                grabbableObject.OnInputActionStarted += e => OnGrabbed(e, wasm.gameObject);
                
                var grabbableObject = wasm.gameObject.GetComponent<GrabbableObject>();
                if (grabbableObject != null)
                {
                    grabbableObject.InputAction = useAction;
                    grabbableObject.OnInputActionStarted += _ => wasm.InvokeOnTouchStart();
                }
                */
            }
            return wasm;
        }

        /*
        private void OnGrabbed(BaseInputEventData e, GameObject gameObject)
        {
            var controller = e.InputSource.Pointers[0].Controller;
            controller.InputSource
            foreach (var inputMapping in controller.Interactions)
            {

            }
            throw new NotImplementedException();
        }
        */

        protected override Transform InstantiatePrimitive(XmlNode node, Transform parent)
        {
            var t = base.InstantiatePrimitive(node, parent);
            if (t != null)
            {
                var gameObject = t.gameObject;
                // gameObject.AddComponent<ObjectManipulator>();
                // gameObject.AddComponent<NearInteractionGrabbable>();
                // var actionHandler = gameObject.AddComponent<GrabbableObject>();
                // actionHandler.InputAction = new MixedRealityInputAction();
            }

            return t;
        }
    }
}
