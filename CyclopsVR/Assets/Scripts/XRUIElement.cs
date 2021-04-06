using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class XRUIElement : MonoBehaviour
{
    protected bool isSelected;

    public bool IsSelected => isSelected;

    protected Interactable currentInteractable;

    public virtual void Select(Interactable interactable)
    {
        currentInteractable = interactable;
        isSelected = true;
    }
    public virtual void Deselect()
    {
        isSelected = false;
        currentInteractable = null;
    }
}
