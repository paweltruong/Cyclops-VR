using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class XRUIElement : MonoBehaviour
{
    protected bool isSelected;

    public bool IsSelected => isSelected;
    
    public virtual void Select()
    {
        isSelected = true;
    }
    public virtual void Deselect()
    {
        isSelected = false;
    }
}
