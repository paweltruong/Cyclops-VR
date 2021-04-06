using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    [SerializeField] bool isUnavailable = true;

    public bool isDisabled;
    protected bool isTargeted;

    public bool IsDisabled => isDisabled;
    public bool IsUnavailable => isUnavailable;
    

    public UnityEvent onSelectionConfirmed;

    public virtual void Targeted()
    {
        isTargeted = true;
    }
    public virtual void Untargeted()
    {
        isTargeted = false;
    }

    public virtual string GetName() => string.Empty;
}
