using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class TestControls : MonoBehaviour
{
    XRInputSubsystem xrInput;

    private void Start()
    {
        var xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null)
        {
            Debug.Log($"XRGeneralSettings is null.");
            return;
        }

        var xrManager = xrSettings.Manager;
        if (xrManager == null)
        {
            Debug.Log($"XRManagerSettings is null.");
            return;
        }

        var xrLoader = xrManager.activeLoader;
        if (xrLoader == null)
        {
            Debug.Log($"XRLoader is null.");
            return;
        }

        Debug.Log($"Loaded XR Device: {xrLoader.name}");

        var xrDisplay = xrLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
        Debug.Log($"XRDisplay: {xrDisplay != null}");

        if (xrDisplay != null)
        {
            if (xrDisplay.TryGetDisplayRefreshRate(out float refreshRate))
            {
                Debug.Log($"Refresh Rate: {refreshRate}hz");
            }
        }

        xrInput = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();
        Debug.Log($"XRInput: {xrInput != null}");

        if (xrInput != null)
        {
            xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
            xrInput.TryRecenter();
        }

        var xrMesh = xrLoader.GetLoadedSubsystem<XRMeshSubsystem>();
        Debug.Log($"XRMesh: {xrMesh != null}");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            XRInputSubsystem subsystem;
            xrInput.TryRecenter();
            //InputTracking.Recenter();
            //Unity.XR.OpenVR.OpenVRHelpers.R
        }
    }
}
