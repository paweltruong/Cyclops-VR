﻿using System.Collections;
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

    GraphicRaycaster m_Raycaster;

    private void Awake()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
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
        CastRay();
    }

    void CastRay()
    {

        Vector3 rayPointerStart = rayOrigin.position;
        Vector3 rayPointerEnd = rayPointerStart +
                                (rayOrigin.forward * maxDistance);

        Vector3 cameraLocation = mainCamera.transform.position;
        Vector3 finalRayDirection = rayPointerEnd - cameraLocation;
        finalRayDirection.Normalize();

        Vector3 finalRayStart = cameraLocation + (finalRayDirection * mainCamera.nearClipPlane);

        var ray = new Ray(finalRayStart, finalRayDirection);

        hitInfo = Physics.RaycastAll(finalRayStart, finalRayDirection, maxDistance, layerMask);

        lastHit = hitInfo.FirstOrDefault();
        bool result = lastHit.collider != null ? lastHit.collider.gameObject.GetComponent<Interactable>() != null : false;

        CastGraphicRay(mainCamera);



#if UNITY_EDITOR        
        Debug.DrawRay(rayPointerStart, rayOrigin.forward * maxDistance, lastHit.collider != null ? Color.green : Color.red);
        DebugUI.UpdateGlobalTarget(string.Join(",", hitInfo.Select(hi => hi.collider.gameObject.name)));
        //if (hitInfo.Length > 0)
        //{
        //    Debug.Log("Hit:" + string.Join(",", hitInfo.Select(hi => hi.collider.gameObject.name)));
        //}
#endif
        var newTarget = result? lastHit.collider.gameObject.GetComponent<Interactable>() : null;
        DebugUI.UpdateGlobalRayState($"TarAq:{targetAquired}, Result:{result}, LastTar:{lastTarget}, LastUI:{lastUIElement}");
        if (lastTarget != newTarget)
            targetAquired = false;//target will change
        //stop focusing on target
        if (targetAquired && !result)
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
                HideDelayed();
            }
        }
        //new target focused
        if (!targetAquired && result)
        {
            targetAquired = true;
            lastTargetCache = lastTarget = newTarget;
            if (lastTarget != null)
            {
                //grow
                Show();
            }
        }


        ////update world UI if something targeted but panel not shown
        //if (targetAquired && result && !worldUI.gameObject.activeInHierarchy && lastTarget != lastHit)
        //{
        //    if()
        //    Show();
        //}

        //when lost focus but user returned to UI that not yet dissapeared
        if (!targetAquired && !result && lastUIElement != null)
        {
            Debug.Log($"Keep UI: {lastTargetCache}");
            //still foucused on UI element
            lastUIElement?.Select(null);
            worldUI.Show(mainCamera, lastTargetCache);
        }
        //hide if nothing is targeted
        if (!targetAquired && !result && lastUIElement == null)
        {
            Debug.Log("Hide delayed");
            HideDelayed();
        }
    }

    void Show()
    {
        if (isReticleShrinked)
            StartCoroutine(ReticleTransition(true));

        lastTarget?.Targeted();
        worldUI.Show(mainCamera, lastTarget);
    }

    void HideDelayed()
    {
        if (!isReticleShrinked)
            StartCoroutine(ReticleTransition(false));

        targetAquired = false;
        if (lastTarget != null)
        {
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
        m_Raycaster.Raycast(m_PointerEventData, results);
        //Debug.Log($"GRay:{m_PointerEventData.position}");

        if (results.Count > 0) Debug.Log("Hit " + results[0].gameObject.name);

        EventSystem.current.RaycastAll(m_PointerEventData, results);
        if (results.Count > 0)
        {
            Debug.Log($"Hit2:{results[0].gameObject.name}");

            if (lastUIElement == null || lastUIElement.gameObject != results[0].gameObject)
            {
                EventSystem.current.SetSelectedGameObject(results[0].gameObject);
                lastUIElement = results[0].gameObject.GetComponent<XRUIElement>();
                lastUIElement?.Select(lastTarget);
                Debug.Log($"State:{(lastUIElement as XRUIButton)?.State}");
            }
        }
        else
        {
            lastUIElement?.Deselect();
            lastUIElement = null;
        }
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
