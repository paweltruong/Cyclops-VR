using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalkingState : IState
{
    WolverineQuotes quotes;
    Animator anim;

    public bool Finished => timer < 0;

    float timer;

    public TalkingState(Animator anim, WolverineQuotes quotes)
    {
        this.anim = anim;
        this.quotes = quotes;
    }

    public void OnEnter()
    {
        anim.SetBool("Talking", true);
        Conversation1.timerAfterStanding = 2f;
        timer =  quotes.Play(WolverineQuotes.WolverineQuotesEnum.YouGonnaTellMeToStayFromYourGirl);
    }

    public void OnExit()
    {
        if(timer<0)
            anim.SetBool("Talking", false);
    }

    public void Tick()
    {
        timer -= Time.deltaTime;
    }
}