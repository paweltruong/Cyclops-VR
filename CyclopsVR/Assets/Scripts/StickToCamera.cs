using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In Unity 2019.3.10f1 for Canvas that is in RenderMode:Screen Point - Camera it is not visible on Android 6, to this script can be attach to Canvas that is world space
public class StickToCamera : MonoBehaviour
{
    [SerializeField] Camera camera;

    float offsetY;
    float offsetZ;
    RectTransform rect;
    Vector3 wordlSpaceOffset;

    private void Awake()
    {
        //rect = GetComponent<RectTransform>();
        //offsetY = rect.position.y;
        //offsetZ = rect.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        //camera.transform.position + camera.transform.forward
    }
}
