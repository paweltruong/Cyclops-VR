using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    [SerializeField] bool isUnavailable = true;

    protected bool isTargeted;

    public bool IsUnavailable => isUnavailable;
    
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
