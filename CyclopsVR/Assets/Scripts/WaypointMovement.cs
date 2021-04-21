using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] float stoppingDistanceErrorMargin = 0.3f;
    [SerializeField] InteractableWaypoint initialWaypoint;

    NavMeshAgent agent;
    InteractableWaypoint previousWaypoint;
    InteractableWaypoint targetedWaypoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        targetedWaypoint = initialWaypoint;
        initialWaypoint.Hide();
        agent.updateRotation = false;
    }

    private void Update()
    {
        var distance = Vector3.Distance(agent.destination, transform.position);
        DebugUI.UpdateGlobalDistanceToWP(distance.ToString());
        //Debug.Log($"Dist:{distance}");
        if (!agent.isStopped)
        {

            if (distance - agent.baseOffset <= agent.stoppingDistance + stoppingDistanceErrorMargin)
            {
                agent.isStopped = true;

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
    }

    public void GoTo(InteractableWaypoint destination)
    {
        previousWaypoint = targetedWaypoint;
        targetedWaypoint = destination;
        targetedWaypoint.isCurrentTarget = true;
        Debug.Log($"GOTO D{targetedWaypoint} , prev:{previousWaypoint}");
        if (destination != null)
        {
            Debug.Log($"Dest , prev:{previousWaypoint}");
            if (previousWaypoint != null)
            {
                //TODO:refactoring
                previousWaypoint.isCurrentTarget = false;
                previousWaypoint.isOccupied = false;
            }
            agent.SetDestination(destination.transform.position);
            targetedWaypoint.Hide();
            targetedWaypoint.ToggleInteractable();
        }
        agent.isStopped = false;
    }
}
