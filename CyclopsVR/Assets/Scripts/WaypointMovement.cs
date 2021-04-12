using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] float speed;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoTo(Transform destination)
    {
        agent.SetDestination(destination.position);
    }
}
