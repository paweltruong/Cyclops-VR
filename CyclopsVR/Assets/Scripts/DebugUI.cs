using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    static DebugUI instance;
    [SerializeField] TextMeshProUGUI txtTarget;
    [SerializeField] TextMeshProUGUI txtDistanceToWP;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }
    public void UpdateTarget(string value)
    {
        txtTarget.text = value;
    }

    public static void UpdateGlobalTarget(string value)
    {
        instance.UpdateTarget(value);
    }

    public void UpdateDistanceToWP(string value)
    {
        txtDistanceToWP.text = value;
    }

    public static void UpdateGlobalDistanceToWP(string value)
    {
        instance.UpdateDistanceToWP(value);
    }
}
