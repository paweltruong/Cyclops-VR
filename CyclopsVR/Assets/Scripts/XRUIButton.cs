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

    public ButtonState State => state;

    public UnityEvent onConfirmed;

    Image image;
    ButtonState state = ButtonState.Default;

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
        UpdateVisuals();
    }

    public override void Select()
    {
        if (state == ButtonState.Default)
        {
            base.Select();
            state = ButtonState.Highlighted;
            StartCoroutine(UpdateConfirmationProgress());
        }
    }

    public override void Deselect()
    {
        if (state != ButtonState.Disabled)
        {
            base.Deselect();
            state = ButtonState.Default;
        }
        progressCanceled = true;
    }

    void UpdateVisuals()
    {
        switch (state)
        {
            case ButtonState.Highlighted:
                Repaint(highlightColor);
                break;
            case ButtonState.Disabled:
                Repaint(disabledColor);
                break;
            case ButtonState.Pressed:
                Repaint(clickedColor);
                break;
            default:
                Repaint(normalColor);
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
        }
        imgProgress.fillAmount = 1;
        if (state != ButtonState.Disabled)
        {
            state = ButtonState.Pressed;
            UpdateVisuals();
            onConfirmed?.Invoke();
        }
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
