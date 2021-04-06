using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class LowPassTracker : MonoBehaviour
{
    [SerializeField] bool isClean = false;
    [SerializeField] float maxCleanFrequency = 5000f;
    [SerializeField] float minCleanEffectFrequency = 3000f;
    [SerializeField] float maxEffectFrequency = 4000f;
    [SerializeField] float minEffectFrequency = 300f;
    [SerializeField] GameObject actor;

    AudioSource audioSource;
    AudioLowPassFilter lowPassFilter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, actor.transform.position);
        if (distance > audioSource.minDistance && audioSource.maxDistance != 0)
        {
            //calculate frequency 
            var frequency = isClean ?
                GetFrequencyByDistance(maxCleanFrequency, minCleanEffectFrequency, distance, audioSource.maxDistance)
                : GetFrequencyByDistance(maxEffectFrequency, minEffectFrequency, distance, audioSource.maxDistance);
            lowPassFilter.cutoffFrequency = frequency;
            Debug.Log($"Dist:{distance} IsClean:{isClean} Freq: {frequency}");
        }
        else
        {
            //rolloff should not be applied but check if there should be clean sound or filtered (f.e closed doors effect)
            lowPassFilter.cutoffFrequency = isClean ? maxCleanFrequency : maxEffectFrequency;
            Debug.Log($"MIN Dist:{distance} IsClean:{isClean} Freq: {lowPassFilter.cutoffFrequency}");
        }
    }

    float GetFrequencyByDistance(float maxFrequency, float minFrequency, float distance, float maxDistance)
    {
        var frequencyRange = maxFrequency - minFrequency;
        var frequencyMultiplier = 1 - distance / maxDistance;
        Debug.Log($"Mulit:{frequencyMultiplier} Range: {frequencyRange}");
        return Mathf.Clamp(maxFrequency * frequencyMultiplier, minFrequency, maxFrequency);
    }
}
