using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LeaningState : IState
{
    CharacterAnimationController character;
    Animator anim;
    public LeaningState(Animator anim, CharacterAnimationController character)
    {
        this.character = character;
        this.anim = anim;
    }

    public void OnEnter()
    {
        anim.SetBool("Leaning", true);
        character.FollowTarget(false);
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
    }
}