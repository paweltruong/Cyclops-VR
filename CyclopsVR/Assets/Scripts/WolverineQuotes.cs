using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolverineQuotes : MonoBehaviour
{
    [SerializeField] AudioClip cyclopsRight;
    [SerializeField] AudioClip iGotBetter;
    [SerializeField] AudioClip nothingToWorry;
    [SerializeField] AudioClip youGonnaTell;
    [SerializeField] AudioClip youWannaGetOut;


    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public enum WolverineQuotesEnum
    {
        CyclopsRight,
        IGotBetterThingsToDo,
        WellThenIGuessYouGotNothingToWorryAboutDoYouCyclops,
        YouGonnaTellMeToStayFromYourGirl,
        YouWannaGetOutOfMyWay
    }

    public float Play(WolverineQuotesEnum quote)
    {
        audioSource.clip = GetQuote(quote);
        audioSource.Play();
        return audioSource.clip.length;
    }

    AudioClip GetQuote(WolverineQuotesEnum quote)
    {
        switch (quote)
        {
            case WolverineQuotesEnum.CyclopsRight:
                return cyclopsRight;
            case WolverineQuotesEnum.IGotBetterThingsToDo:
                return iGotBetter;
            case WolverineQuotesEnum.WellThenIGuessYouGotNothingToWorryAboutDoYouCyclops:
                return nothingToWorry;
            case WolverineQuotesEnum.YouGonnaTellMeToStayFromYourGirl:
                return youGonnaTell;
            case WolverineQuotesEnum.YouWannaGetOutOfMyWay:
                return youWannaGetOut;
            default:
                return null;
                
        }
    }

}
