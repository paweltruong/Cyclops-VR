using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class LowPassTracker : MonoBehaviour
{
    public bool Cutoff { get { return cutoff; } set { cutoff = value; } }
    [SerializeField] bool isClean = false;
    [SerializeField] float maxCleanFrequency = 5000f;
    [SerializeField] float minCleanEffectFrequency = 3000f;
    [SerializeField] float maxEffectFrequency = 4000f;
    [SerializeField] float minEffectFrequency = 300f;
    [SerializeField] GameObject actor;

    [SerializeField] bool cutoff;
    float initialVolume;
    AudioSource audioSource;
    AudioLowPassFilter lowPassFilter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        initialVolume = audioSource.volume;
    }

    private void Update()
    {
        if (Cutoff)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = initialVolume;

            var distance = Vector3.Distance(transform.position, actor.transform.position);
            if (distance > audioSource.minDistance && audioSource.maxDistance != 0)
            {
                //calculate frequency 
                var frequency = isClean ?
                    GetFrequencyByDistance(maxCleanFrequency, minCleanEffectFrequency, distance, audioSource.maxDistance)
                    : GetFrequencyByDistance(maxEffectFrequency, minEffectFrequency, distance, audioSource.maxDistance);
                lowPassFilter.cutoffFrequency = frequency;
            }
            else
            {
                //rolloff should not be applied but check if there should be clean sound or filtered (f.e closed doors effect)
                lowPassFilter.cutoffFrequency = isClean ? maxCleanFrequency : maxEffectFrequency;
            }
        }
    }

    float GetFrequencyByDistance(float maxFrequency, float minFrequency, float distance, float maxDistance)
    {
        var frequencyRange = maxFrequency - minFrequency;
        var frequencyMultiplier = 1 - distance / maxDistance;
        return Mathf.Clamp(maxFrequency * frequencyMultiplier, minFrequency, maxFrequency);
    }
}
