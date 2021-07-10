using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserBeamManager : MonoBehaviour
{
    [Tooltip("After target is locked, will fire laser beam after this countdown unless untargeted (in seconds)")]
    [SerializeField] float gazeFireCountdown = 2f;
    [SerializeField] AudioClip beamSound;
    [SerializeField] GameObject laserVFX;
    [SerializeField] Camera mainCamera;
    [SerializeField] int damagePerTick = 5;

    AudioSource audioSource;

    List<InteractableEnemy> gazeTargets = new List<InteractableEnemy>();
    float gazeFireCountdownValue;
    bool beamShouldBeCharging = false;
    bool beamActive = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (laserVFX != null)
            laserVFX.SetActive(false);
    }

    void Update()
    {
        if (beamShouldBeCharging)
        {
            if (gazeFireCountdownValue > 0)
            {
                //tick
                gazeFireCountdownValue -= Time.deltaTime;
                //Debug.Log($"[LBM]Beam countdown: {gazeFireCountdownValue}");
            }
            else
            {
                //fire
                gazeFireCountdownValue = gazeFireCountdown;
                Fire();
            }
        }
    }

    void Fire()
    {
        if (audioSource != null && beamSound != null)
            audioSource.PlayOneShot(beamSound);

        StartCoroutine(DisplayRay(1f));
    }



    IEnumerator DisplayRay(float duration)
    {
        if (laserVFX != null)
        {
            laserVFX.SetActive(true);
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                var mousePos = Input.mousePosition;
                var rayMouse = mainCamera.ScreenPointToRay(mousePos);
                laserVFX.transform.LookAt(rayMouse.GetPoint(10f));

                for(int i=0;i<gazeTargets.Count;++i)
                    gazeTargets[i].OnHit(damagePerTick);
                yield return null;
            }
        }
    }



    internal void StartFireCountdown(InteractableEnemy target)
    {
        Debug.Log($"[LBM]Target {target} aquired");
        if (!gazeTargets.Any(t => t.GetName() == target.GetName()))
        {
            gazeTargets.Add(target);
            if (!beamShouldBeCharging)
            {
                beamShouldBeCharging = true;
                gazeFireCountdownValue = gazeFireCountdown;
            }
        }
    }

    internal void CancelFireCountdown(InteractableEnemy target)
    {
        Debug.Log($"[LBM]Target {target} lost");
        if (gazeTargets.Any(t => t.GetName() == target.GetName()))
        {
            gazeTargets.Remove(target);
            if (!gazeTargets.Any())
            {
                beamShouldBeCharging = false;
                gazeFireCountdownValue = gazeFireCountdown;

                if (laserVFX != null)
                    laserVFX.SetActive(false);
            }
        }
    }
}
