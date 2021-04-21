using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class XRUIElement : MonoBehaviour
{
    protected bool isSelected;

    public bool IsSelected => isSelected;

    protected Interactable currentInteractableCache;
    protected Interactable currentInteractable;

    /// <summary>
    /// </summary>
    /// <param name="interactable">when null select last bound interactable object</param>
    public virtual void Select(Interactable interactable)
    {
        if (interactable == null)
            currentInteractable = currentInteractableCache;
        else
            currentInteractableCache = currentInteractable = interactable;

        isSelected = true;
    }
    public virtual void Deselect()
    {
        isSelected = false;
        currentInteractable = null;
    }
}
