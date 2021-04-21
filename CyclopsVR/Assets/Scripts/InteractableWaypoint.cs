using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InteractableWaypoint : Interactable
{
    [SerializeField] string key;
    [SerializeField] string waypointName;
    [SerializeField] GameObject waypointVisualization;

    [SerializeField] InteractableDoor[] DoorsInRoom;

    public UnityEvent onReached;

    /// <summary>
    /// all doors are closed
    /// </summary>
    public UnityEvent onRoomSealed;
    /// <summary>
    ///  not all doors are closed
    /// </summary>
    public UnityEvent onRoomBreached;

    public bool isCurrentTarget;
    public bool isOccupied;

    bool isSealed = true;

    public override string GetName()
    {
        return waypointName;
    }

    private void Start()
    {
        if (isOccupied)
            Hide();
        else
            Show();

        if (DoorsInRoom != null)
            foreach (var door in DoorsInRoom)
            {
                door.onDoorClosed.AddListener(UpdateRoomStatus);
                door.onDoorOpening.AddListener(UpdateRoomStatus);
            }
    }

    void UpdateRoomStatus()
    {
        bool allSealed = true;
        if (DoorsInRoom != null)
            foreach (var door in DoorsInRoom)
            {
                if (door.status != DoorStatus.Closed)
                {
                    allSealed = false;
                    break;
                }
            }

        //if player is inside the room
        if (isCurrentTarget)
        {
            //invoke when state changed
            if (allSealed && !isSealed)
                onRoomSealed?.Invoke();
            else if (!allSealed && isSealed)
                onRoomBreached.Invoke();
        }

        isSealed = allSealed;
    }


    public void Hide()
    {
        waypointVisualization.SetActive(false);
    }
    public void Show()
    {
        waypointVisualization.SetActive(true);
    }
}
