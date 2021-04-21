using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    static DebugUI instance;
    [SerializeField] TextMeshProUGUI txtTarget;
    [SerializeField] TextMeshProUGUI txtDistanceToWP;
    [SerializeField] TextMeshProUGUI txtRayState;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);

        if (txtTarget == null || txtDistanceToWP == null || txtRayState == null)
            Debug.LogError("Debug text fields not set");
    }
    public void UpdateField(TextMeshProUGUI txtField, string value)
    {
        txtField.text = value;
    }

    public static void UpdateGlobalTarget(string value)
    {
        instance?.UpdateField(instance?.txtTarget, value);
    }

    public static void UpdateGlobalDistanceToWP(string value)
    {
        instance?.UpdateField(instance?.txtDistanceToWP, value);
    }
    public static void UpdateGlobalRayState(string value)
    {
        instance?.UpdateField(instance?.txtRayState, value);
    }
}
