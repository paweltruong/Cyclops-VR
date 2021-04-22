using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform rayOrigin;
    [SerializeField] Image reticleOutline;
    [SerializeField] Image reticleFill;
    [SerializeField] float maxDistance = 100f;
    [SerializeField] LayerMask layerMask = ~0;
    [SerializeField] float minSize = 10f;
    [SerializeField] float maxSize = 30f;
    [SerializeField] float transitionDuration = .4f;
    [SerializeField] WorldUI worldUI;

    RaycastHit[] hitInfo;
    Ray lastRay;
    RaycastHit lastHit;
    Interactable lastTargetCache;
    Interactable lastTarget;
    XRUIElement lastUIElement;
    bool targetAquired = false;
    RectTransform rect;
    bool isReticleShrinked;

    GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (reticleOutline == null || reticleFill == null)
            Debug.LogError("Reticles not set");

        if (worldUI == null)
            Debug.LogError("UI not set");

        if (mainCamera == null)
            Debug.LogError("Camera is not set");

        if (rayOrigin == null)
            rayOrigin = mainCamera.transform;
    }

    void Update()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        bool physicRayResult;

        CastPhysicRay(out physicRayResult);
        CastGraphicRay(mainCamera);

        var newTarget = physicRayResult ? lastHit.collider.gameObject.GetComponent<Interactable>() : null;

        DebugUI.UpdateGlobalRayState($"TarAq:{targetAquired}, Result:{physicRayResult}, LastTar:{lastTarget}, LastUI:{lastUIElement}");

        HandleRays(physicRayResult, newTarget);
    }

    /// <summary>
    /// TODO:refactoring
    /// </summary>
    /// <param name="physicRayResult"></param>
    /// <param name="newTarget"></param>
    private void HandleRays(bool physicRayResult, Interactable newTarget)
    {
        if (lastTarget != newTarget)
            targetAquired = false;//target will change
        //stop focusing on target
        if (targetAquired && !physicRayResult)
        {
            if (lastUIElement != null)
            {
                //if UI is still focused even if interactable object is not targeted any more
                //dont hide - do nothing
            }
            else
            {
                //hide menu
                //shrink
                HideUIAndSelectionDelayed();
            }
        }
        //new target focused
        if (!targetAquired && physicRayResult)
        {
            targetAquired = true;
            lastTargetCache = lastTarget = newTarget;
            if (lastTarget != null)
            {
                //grow
                ShowUIAndSelection();
            }
        }

        //when lost focus but user returned to UI that not yet dissapeared
        if (!targetAquired && !physicRayResult && lastUIElement != null)
        {
            //Debug.Log($"Keep UI: {lastTargetCache}");
            //still foucused on UI element
            lastUIElement?.Select(null);
            worldUI.Show(mainCamera, lastTargetCache);
            TrySetWaypointHighlight(lastTargetCache, true);
        }
        //hide if nothing is targeted
        if (!targetAquired && !physicRayResult && lastUIElement == null)
        {
            //Debug.Log("Hide delayed");
            HideUIAndSelectionDelayed();
        }
    }

    private void CastPhysicRay( out bool result)
    {
        Vector3 rayPointerStart = rayOrigin.position;
        Vector3 rayPointerEnd = rayPointerStart + (rayOrigin.forward * maxDistance);

        Vector3 cameraLocation = mainCamera.transform.position;
        Vector3 finalRayDirection = rayPointerEnd - cameraLocation;
        finalRayDirection.Normalize();

        Vector3 finalRayStart = cameraLocation + (finalRayDirection * mainCamera.nearClipPlane);

        var ray = new Ray(finalRayStart, finalRayDirection);

        hitInfo = Physics.RaycastAll(finalRayStart, finalRayDirection, maxDistance, layerMask);

        //last hit is the closest hit
        lastHit = hitInfo.OrderBy(hi => Vector3.Distance(hi.point, this.transform.position)).FirstOrDefault();

        result = lastHit.collider != null ? lastHit.collider.gameObject.GetComponent<Interactable>() != null : false;

#if UNITY_EDITOR
        Debug.DrawRay(rayPointerStart, rayOrigin.forward * maxDistance, lastHit.collider != null ? Color.green : Color.red);
        DebugUI.UpdateGlobalTarget(string.Join(",", hitInfo.Select(hi => hi.collider.gameObject.name)));
#endif
    }

    void ShowUIAndSelection()
    {
        if (isReticleShrinked)
            StartCoroutine(ReticleTransition(true));

        lastTarget?.Targeted();
        worldUI.Show(mainCamera, lastTarget);

        TrySetWaypointHighlight(lastTarget, true);
    }

    void HideUIAndSelectionDelayed()
    {
        if (!isReticleShrinked)
            StartCoroutine(ReticleTransition(false));

        targetAquired = false;
        if (lastTarget != null)
        {
            TrySetWaypointHighlight(lastTarget, false);

            lastTarget.Untargeted();
            lastTarget = null;
        }
        worldUI.HideDelayed();
    }

    void CastGraphicRay(Camera camera)
    {
        var m_EventSystem = EventSystem.current;
        //Set up the new Pointer Event
        var m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = camera.WorldToScreenPoint(this.transform.position);

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        graphicRaycaster.Raycast(m_PointerEventData, results);
        //Debug.Log($"GRay:{m_PointerEventData.position}");

        if (results.Count > 0) Debug.Log("Hit " + results[0].gameObject.name);

        EventSystem.current.RaycastAll(m_PointerEventData, results);
        if (results.Count > 0)
        {
            //Debug.Log($"Hit2:{results[0].gameObject.name}");

            if (lastUIElement == null || lastUIElement.gameObject != results[0].gameObject)
            {
                EventSystem.current.SetSelectedGameObject(results[0].gameObject);
                lastUIElement = results[0].gameObject.GetComponent<XRUIElement>();
                lastUIElement?.Select(lastTarget);
                //Debug.Log($"State:{(lastUIElement as XRUIButton)?.State}");
            }
        }
        else
        {
            lastUIElement?.Deselect();
            lastUIElement = null;
        }
    }

    void TrySetWaypointHighlight(Interactable interactable, bool highlightEnabled)
    {
        //TODO:add delayed support
        //handle waypoint specific
        if (interactable is InteractableWaypoint)
            (interactable as InteractableWaypoint).SetHighlight(highlightEnabled);
    }

    /// <summary>
    /// grow or shrink croshair
    /// </summary>
    /// <param name="grow"></param>
    /// <returns></returns>
    IEnumerator ReticleTransition(bool grow)
    {
        float counter = transitionDuration;
        var range = maxSize - minSize;
        float progress = grow ? 0 : 1;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            progress = (transitionDuration - counter) / transitionDuration;


            if (grow)
            {
                reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minSize + progress * range);
                reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minSize + progress * range);
            }
            else
            {
                reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxSize - progress * range);
                reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxSize - progress * range);
            }
            reticleOutline.rectTransform.ForceUpdateRectTransforms();
            yield return new WaitForFixedUpdate();
        }
        counter = 0;

        if (grow)
        {
            reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxSize);
            reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxSize);
        }
        else
        {
            reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minSize);
            reticleOutline.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minSize);
        }
        isReticleShrinked = !grow;
    }
}
