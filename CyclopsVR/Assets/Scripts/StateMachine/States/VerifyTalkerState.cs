using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VerifyTalkerState : IState
{
    CharacterAnimationController character;
    Transform lookAtTarget;
    float lookAtWeight;
    WolverineQuotes quotes;

    public VerifyTalkerState(CharacterAnimationController character, WolverineQuotes quotes, Transform lookAtTarget, float lookAtWeight)
    {
        this.character = character;
        this.lookAtTarget = lookAtTarget;
        this.lookAtWeight = lookAtWeight;
        this.quotes = quotes;
    }

    public void OnEnter()
    {
        character.FollowTarget(true, lookAtTarget, lookAtWeight);
        quotes.Play(WolverineQuotes.WolverineQuotesEnum.CyclopsRight);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {

    }
}