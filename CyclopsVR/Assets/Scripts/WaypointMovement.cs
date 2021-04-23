using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class WaypointMovement : NodeMovement
{
    [SerializeField] float stoppingDistanceErrorMargin = 0.3f;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        base.Initialize();
        agent.updateRotation = false;
    }

    private void Update()
    {
        if (GameSettings.globalLocomotion == LocomotionType.AutoWalk)
        {
            var distance = Vector3.Distance(agent.destination, transform.position);
            DebugUI.UpdateGlobalDistanceToWP(distance.ToString());
            //Debug.Log($"Dist:{distance}");
            if (!agent.isStopped)
            {

                if (distance - agent.baseOffset <= agent.stoppingDistance + stoppingDistanceErrorMargin)
                {
                    agent.isStopped = true;

                    Arrived();
                }
            }
        }
    }


    public override void GoTo(InteractableWaypoint destination)
    {
        base.GoTo(destination);

        if (destination != null)
            agent.SetDestination(destination.transform.position);

        agent.isStopped = false;
    }
}
