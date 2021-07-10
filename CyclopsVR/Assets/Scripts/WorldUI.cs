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
    /// <summary>
    /// open close door
    /// </summary>
    [SerializeField] XRUIButton btnDoor;
    /// <summary>
    /// walk into place
    /// </summary>
    [SerializeField] XRUIButton btnWalk;
    /// <summary>
    /// spawn enemies
    /// </summary>
    [SerializeField] XRUIButton btnBattle;
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
        if (btnDoor == null)
            Debug.LogError("Door button field not set");
        if (btnWalk == null)
            Debug.LogError("Walk button field not set");
        if (btnBattle == null)
            Debug.LogError("Battle button field not set");
    }

    public void HideDelayed()
    {
        if (this.gameObject.activeInHierarchy)
        {
            hideCanceled = false;
            StartCoroutine(HidingTask());
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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

                if (interactable is InteractableDoor)
                {
                    btnDoor.gameObject.SetActive(!interactable.IsUnavailable);
                    btnWalk.gameObject.SetActive(false);
                    btnBattle.gameObject.SetActive(false);
                }
                else if (interactable is InteractableWaypoint)
                {
                    btnDoor.gameObject.SetActive(false);
                    btnWalk.gameObject.SetActive(true);
                    btnBattle.gameObject.SetActive(false);
                }
                else if (interactable is InteractableBattle)
                {
                    btnDoor.gameObject.SetActive(false);
                    btnWalk.gameObject.SetActive(false);
                    btnBattle.gameObject.SetActive(true);
                }
                else
                {
                    btnDoor.gameObject.SetActive(false);
                    btnWalk.gameObject.SetActive(false);
                    btnBattle.gameObject.SetActive(false);
                }

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
