using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StandingState : IState
{
    Animator anim;
    public StandingState(Animator anim)
    {
        this.anim = anim;
    }

    public void OnEnter()
    {
        anim.SetBool("Leaning", false);
        Conversation1.timerAfterStanding = 2f;
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        Conversation1.timerAfterStanding -= Time.deltaTime;
    }
}