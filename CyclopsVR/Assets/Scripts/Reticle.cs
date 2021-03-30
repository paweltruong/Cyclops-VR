using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    Ray lastRay;
    RaycastHit lastHit;
    bool targetAquired = false;

    private void Start()
    {
        if (reticleOutline == null || reticleFill == null)
            Debug.LogError("Reticles not set");

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
        RaycastHit hitInfo;
        var result = Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
        lastHit = hitInfo;

#if UNITY_EDITOR        
        Debug.DrawRay(rayPointerStart, rayOrigin.forward * maxDistance, result ? Color.green : Color.red);
#endif
        if (targetAquired && !result)
        {
            //shrink
            StartCoroutine(Transition2(false));
            targetAquired = false;
        }
        if (!targetAquired && result)
        {
            //grow
            StartCoroutine(Transition2(true));
            targetAquired = true;
        }
    }

    IEnumerator Transition2(bool grow)
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
    }
}
