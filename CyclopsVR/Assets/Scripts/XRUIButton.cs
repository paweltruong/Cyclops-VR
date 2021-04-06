using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class XRUIButton : XRUIElement
{
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color highlightColor = Color.green;
    [SerializeField] Color highlightFillColor = Color.blue;
    [SerializeField] Color clickedColor = Color.yellow;
    [SerializeField] Color disabledColor = Color.grey;
    [SerializeField] Image[] highlightableChildren;
    [SerializeField] Image imgProgress;
    [Tooltip("Time in seconds before button press is confirmed")]
    [SerializeField] float confirmationDuration = .5f;
    [SerializeField] bool hideOnDisabled = true;

    public ButtonState State => state;

    public UnityEvent onConfirmed;

    Image image;
    public ButtonState state = ButtonState.Default;

    bool progressCanceled = false;

    private void Awake()
    {
        image = GetComponent<Image>();

        if (image == null)
            Debug.LogError($"{gameObject.name} does not have UI.Image");
        if (imgProgress == null)
            Debug.LogError($"{gameObject.name} UI.Image for focus progress not set");
        imgProgress.fillAmount = 0;
        imgProgress.color = highlightFillColor;
    }

    public void DisableButton()
    {
        state = ButtonState.Disabled;
        progressCanceled = true;
        imgProgress.fillAmount = 0;
    }

    public override void Select(Interactable interactable)
    {
        if (!CheckInteractableDisabled())
        {
            if (state == ButtonState.Default)
            {
                base.Select(interactable);

                state = ButtonState.Highlighted;
                StartCoroutine(UpdateConfirmationProgress());
            }
        }
    }

    bool CheckInteractableDisabled()
    {
        if (currentInteractable != null && currentInteractable.IsDisabled)
        {
            state = ButtonState.Disabled;
            return true;
        }
        return false;
    }

    public override void Deselect()
    {
        if(!CheckInteractableDisabled())
        {
            base.Deselect();

            state = ButtonState.Default;
        }
        progressCanceled = true;
    }

    private void LateUpdate()
    {
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (CheckInteractableDisabled())
        {
            if (hideOnDisabled)
            {
                ToggleVisualization(false);
            }

            return;
        }
        else
        {
            if (state == ButtonState.Disabled)
                state = ButtonState.Default;
            ToggleVisualization(true);
        }
            

        switch (state)
        {
            case ButtonState.Highlighted:
                Repaint(highlightColor);
                break;
            case ButtonState.Disabled:
                Repaint(disabledColor);
                imgProgress.fillAmount = 0;
                break;
            case ButtonState.Pressed:
                Repaint(clickedColor);
                imgProgress.fillAmount = 0;
                break;
            default:
                Repaint(normalColor);
                imgProgress.fillAmount = 0;
                break;
        }
    }

    void Repaint(Color color)
    {
        image.color = color;
        if (highlightableChildren != null)
            foreach (var img in highlightableChildren)
            {
                if (img != null)
                    img.color = color;
            }
    }

    void ToggleVisualization(bool enabled)
    {
        image.enabled = enabled;
        if (highlightableChildren != null)
            foreach (var img in highlightableChildren)
            {
                if (img != null)
                    img.enabled = enabled;
            }
    }

    IEnumerator UpdateConfirmationProgress()
    {
        progressCanceled = false;
        float counter = confirmationDuration;
        float progress;
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            progress = (confirmationDuration - counter) / confirmationDuration;

            imgProgress.fillAmount = progress;
            yield return new WaitForFixedUpdate();
            if (progressCanceled)
            {
                imgProgress.fillAmount = 0;
                progressCanceled = false;
                UpdateVisuals();
                yield break;
            }
            if(CheckInteractableDisabled())
                UpdateVisuals();
        }
        imgProgress.fillAmount = 1;
        if (state != ButtonState.Disabled)
        {
            state = ButtonState.Pressed;
            UpdateVisuals();
            Confirm();
        }
    }

    void Confirm()
    {
        Debug.Log("Confirm");
        onConfirmed?.Invoke();
        currentInteractable.onSelectionConfirmed?.Invoke();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (image != null)
        {
            UpdateVisuals();
            imgProgress.color = highlightFillColor;
        }
    }
#endif
}
