using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    [SerializeField] bool isUnavailable = true;
    [SerializeField] Collider collider;

    public bool isDisabled;
    public bool isInteractable = true;
    protected bool isTargeted;

    public bool IsDisabled => isDisabled;
    public bool IsUnavailable => isUnavailable;
    

    public UnityEvent onSelectionConfirmed;

    private void Awake()
    {
        if (collider == null)
            Debug.LogError("Collider is not set");
    }

    public virtual void Targeted()
    {
        isTargeted = true;
    }
    public virtual void Untargeted()
    {
        isTargeted = false;
    }

    public virtual string GetName() => string.Empty;

    public void ToggleInteractable()
    {
        SetInteractable(!isInteractable);
    }

    public void SetInteractable(bool value)
    {
        isInteractable = value;
        collider.enabled = value;
    }
}
