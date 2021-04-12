using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldUI : MonoBehaviour
{
    [SerializeField] float distanceFromCamera;
    [SerializeField] float distanceFromGround;
    [SerializeField] bool isBillboard;

    [SerializeField] TextMeshProUGUI txtNameField;
    [SerializeField] TextMeshProUGUI txtUnavailableField;
    [SerializeField] XRUIButton btnEnterRoom;
    [Tooltip("Hide UI for object after x seconds if not targeted")]
    [SerializeField] float hideDelay = 1f;


    public float DistanceFromCamera => distanceFromCamera;
    public float DistanceFromGround => distanceFromGround;

    bool hideCanceled;
    Camera camera;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (txtNameField == null)
            Debug.LogError("Name text field not set");
        if (txtUnavailableField == null)
            Debug.LogError("Unavailable text field not set");
        if (btnEnterRoom == null)
            Debug.LogError("EnterRoom button field not set");
    }

    public void Hide()
    {
        if (this.gameObject.activeInHierarchy)
        {
            hideCanceled = false;
            StartCoroutine(HidingTask());
        }
    }

    IEnumerator HidingTask()
    {
        yield return new WaitForSeconds(hideDelay);
        if (!hideCanceled)
            gameObject.SetActive(false);
    }

    public void Show(Camera camera, Interactable interactable)
    {
        if (interactable != null)
        {
            if (interactable.isInteractable)
            {
                hideCanceled = true;
                var positionOfTarget = interactable.transform.position;
                this.camera = camera;
                gameObject.SetActive(true);


                var forwardCenter = positionOfTarget - camera.transform.position;
                var newPosition = camera.transform.position + forwardCenter.normalized * distanceFromCamera;
                newPosition.y = distanceFromGround;
                transform.position = newPosition;

                var correctedCameraPosition = camera.transform.position;
                correctedCameraPosition.y = distanceFromGround;

                transform.rotation = Quaternion.LookRotation(transform.position - correctedCameraPosition);
                txtNameField.text = interactable.GetName();
                txtUnavailableField.gameObject.SetActive(interactable.IsUnavailable);
                btnEnterRoom.gameObject.SetActive(!interactable.IsUnavailable);
            }
        }
        else
            Debug.LogError("Interactable is null");
    }

    private void LateUpdate()
    {
        if (isBillboard)
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
