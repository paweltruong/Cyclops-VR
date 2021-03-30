using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    protected bool isTargeted;
    
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
