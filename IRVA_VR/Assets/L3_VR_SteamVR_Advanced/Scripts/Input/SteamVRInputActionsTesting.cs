using System;
using UnityEngine;
using Valve.VR;

namespace L3_VR_SteamVR_Advanced.Scripts.Input
{
    public class SteamVRInputActionsTesting : MonoBehaviour
    {
        // TODO 1 : Setup input for the already-defined `GrabPinch` action.
        //          Write a message in the console which signifies this input is correctly read.
        //          Use either the polling method or an event-based mechanism.
        private void OnEnable()
        {
            SteamVR_Actions._default.GrabPinch.onChange += OnGrabPinchChanged;
            SteamVR_Actions._default.TouchTrigger.onChange += OnTouchTriggerChanged;
        }
        
        private void OnDisable()
        {
            SteamVR_Actions._default.GrabPinch.onChange -= OnGrabPinchChanged;
            SteamVR_Actions._default.TouchTrigger.onChange -= OnTouchTriggerChanged;
        }
 
        // Method called when `onChange` from the `GrabPinch` is invoked.
        private void OnGrabPinchChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool grabPinchState)
        {
            Debug.Log($"[SteamVRInputActionsTesting] Events: grabPinchState = {grabPinchState}");
        }

        // TODO 2 : Setup input for the `TouchTrigger` action (you'll have to first create it & bind it accordingly)
        //          Write a message in the console which signifies this input is correctly read.
        //          Use either the polling method or an event-based mechanism.
 
        // Method called when `onChange` from the `TouchTrigger` is invoked.
        private void OnTouchTriggerChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool touchTriggerState)
        {
            Debug.Log($"[SteamVRInputActionsTesting] Events: touchTriggerState = {touchTriggerState}");
        }
    }
}