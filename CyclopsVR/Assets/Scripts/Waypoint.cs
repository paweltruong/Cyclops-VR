
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Waypoint : MonoBehaviour
{
    [SerializeField] string Key;

    public UnityEvent onReached;
}
