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

    public UnityEvent onReached;

    public bool isOccupied;    

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
