using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationMovement : NodeMovement
{
    [SerializeField] ScreenFader fader;

    private void Start()
    {
        base.Initialize();
        if (fader == null)
            Debug.LogError("Screen fader not set");
    }

    public override void GoTo(InteractableWaypoint destination)
    {
        base.GoTo(destination);

        fader.FadeOut();
        Invoke(nameof(Teleport), fader.Duration);
    }

    void Teleport()
    {
        transform.position = targetedWaypoint.transform.position;
        Arrived();
        fader.FadeIn();
    }
}
