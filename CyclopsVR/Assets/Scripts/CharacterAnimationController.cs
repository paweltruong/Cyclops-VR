using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    Animator anim;
    Transform lookAtTarget;
    float lookAtWeight;
    bool followEnabled;

    void Start()
    {

        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (followEnabled)
        {
            anim.SetLookAtPosition(lookAtTarget.position);
            anim.SetLookAtWeight(lookAtWeight);
        }
    }

    public void FollowTarget(bool enabled, Transform target = null, float weight = 0f)
    {
        followEnabled = enabled;
        if(followEnabled)
        {
            lookAtTarget = target;
            lookAtWeight = weight;
        }
    }

}
