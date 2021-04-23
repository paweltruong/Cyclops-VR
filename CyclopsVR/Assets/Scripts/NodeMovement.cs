using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class NodeMovement : MonoBehaviour
{

    [SerializeField] protected InteractableWaypoint initialWaypoint;

    protected InteractableWaypoint previousWaypoint;
    protected InteractableWaypoint targetedWaypoint;

    protected void Initialize()
    {
        targetedWaypoint = initialWaypoint;
        initialWaypoint.Hide();
        Debug.Log($"Start:{GetType().Name}");
    }

    public virtual void GoTo(InteractableWaypoint destination)
    {
        previousWaypoint = targetedWaypoint;
        targetedWaypoint = destination;
        targetedWaypoint.isCurrentTarget = true;
        Debug.Log($"GOTO D{targetedWaypoint} , prev:{previousWaypoint}");
        if (targetedWaypoint != null)
        {
            Debug.Log($"Dest , prev:{previousWaypoint}");
            if (previousWaypoint != null)
            {
                //TODO:refactoring
                previousWaypoint.isCurrentTarget = false;
                previousWaypoint.isOccupied = false;
            }

            targetedWaypoint.Hide();
            targetedWaypoint.ToggleInteractable();
        }
    }

    public virtual void Arrived()
    {
        if (previousWaypoint != null)
        {
            previousWaypoint.Show();
            previousWaypoint.ToggleInteractable();
        }

        if (targetedWaypoint != null
            && previousWaypoint != null//ignore start of the level, initial waypoint
            )
        {
            targetedWaypoint.isOccupied = true;
            targetedWaypoint?.onReached?.Invoke();
        }
    }
}
