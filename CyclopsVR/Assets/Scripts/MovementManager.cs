using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    WaypointMovement waypointMovement;
    TeleportationMovement teleportationMovement;

    private void Awake()
    {
        waypointMovement = GetComponent<WaypointMovement>();
        teleportationMovement = GetComponent<TeleportationMovement>();

        if (waypointMovement == null || teleportationMovement == null)
            Debug.LogError("Movement scripts not attached");
    }

    public void GoTo(InteractableWaypoint destination)
    {
        switch (GameSettings.globalLocomotion)
        {
            case LocomotionType.AutoWalk:
                teleportationMovement.enabled = false;
                waypointMovement.GoTo(destination);
                break;
            default:
                waypointMovement.enabled = false;
                teleportationMovement.GoTo(destination);
                break;
        }
    }
}
