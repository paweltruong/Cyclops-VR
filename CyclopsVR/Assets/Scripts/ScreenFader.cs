using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFader : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    Image img;

    public float Duration => duration;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void FadeIn()
    {
        Color startColor, endColor;
        startColor = endColor= img.color;

        startColor.a = 1;
        endColor.a = 0;

        StartCoroutine(FadeColor(startColor, endColor, duration));
    }

    public void FadeOut()
    {
        Color startColor, endColor;
        startColor = endColor = img.color;

        startColor.a = 0;
        endColor.a = 1;

        StartCoroutine(FadeColor(startColor, endColor, duration));
    }

    IEnumerator FadeColor(Color start, Color end, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;

            img.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        img.color = end;
    }
}
