using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] LocomotionType locomotion = LocomotionType.Teleportation;

    //TODO:move to player prefs?
    internal static LocomotionType globalLocomotion;

    private void OnValidate()
    {
        globalLocomotion = locomotion;
        //Debug.Log($"Locomotion set to: {globalLocomotion}");
    }
}
