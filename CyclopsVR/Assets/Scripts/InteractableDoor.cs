using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class InteractableDoor : Interactable
{
    [SerializeField] string roomName;    

    public override string GetName()
    {
        return roomName;
    }
}
