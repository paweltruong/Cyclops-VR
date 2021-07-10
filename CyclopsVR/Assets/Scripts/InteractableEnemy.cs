using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class InteractableEnemy : Interactable
{
    [SerializeField] string id = Guid.NewGuid().ToString();
    [SerializeField] int health = 100;
    public int actualHealth;

    LaserBeamManager laserBeamManager;

    public override string GetName()
    {
        return base.GetName();
    }

    private void Start()
    {
        laserBeamManager = FindObjectOfType<LaserBeamManager>();
        ResetHP();
    }

    public void ResetHP()
    {
        actualHealth = health;
    }

    public override void Targeted()
    {
        base.Targeted();
        laserBeamManager.StartFireCountdown(this);
    }

    public override void Untargeted()
    {
        base.Untargeted();
        laserBeamManager.CancelFireCountdown(this);
        
    }

    public void OnHit(int dmg)
    {
        actualHealth -= dmg;
        if (actualHealth <= 0)
        {
            laserBeamManager.CancelFireCountdown(this);
            this.gameObject.SetActive(false);
        }
    }
}
