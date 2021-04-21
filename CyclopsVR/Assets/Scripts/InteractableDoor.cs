using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class InteractableDoor : Interactable
{
    [SerializeField] string roomName;
    public DoorStatus status;
    public UnityEvent onDoorOpened;
    public UnityEvent onDoorOpening;
    public UnityEvent onDoorClosed;

    Animator anim;
    AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public override string GetName()
    {
        return roomName;
    }

    public void Open()
    {
        if (status != DoorStatus.Open || status != DoorStatus.Opening)
        {
            anim.SetTrigger("Open");
            audioSource.Play();
        }
    }

    public void Close()
    {
        if (status != DoorStatus.Closed || status != DoorStatus.Closing)
        {
            anim.SetTrigger("Close");
            audioSource.Play();
        }
    }

    public void ToggleDoor()
    {
        if (status == DoorStatus.Closed)
            Open();
        else if (status == DoorStatus.Open)
            Close();
    }

    public void UpdateDoorStatus(DoorStatus status)
    {
        this.status = status;
        switch (status)
        {
            case DoorStatus.Opening:
                onDoorOpening?.Invoke();
                isDisabled = true;
                break;
            case DoorStatus.Closing:
                isDisabled = true;
                break;
            case DoorStatus.Open:
                isDisabled = false;
                onDoorOpened?.Invoke();
                break;
            default:
                onDoorClosed?.Invoke();
                isDisabled = false;
                break;
        }
    }
}
