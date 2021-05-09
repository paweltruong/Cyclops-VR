using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation1 : MonoBehaviour
{
    [SerializeField] MicInput micInput;
    [SerializeField] NodDetector nodDetector;
    [SerializeField] float talkingTimeThreshold = 0.1f;

    StateMachine _stateMachine;

    [SerializeField] CharacterAnimationController wolverine;

    [SerializeField] Transform lookAtTarget;
    [SerializeField] float lookAtWeight;
    [SerializeField] WolverineQuotes wolverineQuotes;

    Animator wolverineAnim;

    public static float timerAfterStanding = 2f;

    void Start()
    {
        if (micInput == null)
            Debug.LogError("MicINput not set");

        wolverineAnim = wolverine.GetComponent<Animator>();

        _stateMachine = new StateMachine();

        var leaning = new LeaningState(wolverineAnim, wolverine);
        var areYouTalkingToMe = new VerifyTalkerState(wolverine, wolverineQuotes, lookAtTarget, lookAtWeight);
        var standing = new StandingState(wolverineAnim);
        var talking = new TalkingState(wolverineAnim, wolverineQuotes);

        //At
        At(areYouTalkingToMe, leaning, IsTalking);
        At(leaning, areYouTalkingToMe, IsShaking);
        At(standing, areYouTalkingToMe, IsNoding);
        At(talking, standing, DramaticPausePassed);
        At(leaning, talking, FinishedTalking);

        //At()

        _stateMachine.SetState(leaning);
    }
    

    void Update()
    {
        _stateMachine.Tick();
        if (IsTalking())
            Debug.Log("Talking");
    }

    bool IsTalking() => micInput.talkingTime > talkingTimeThreshold;
    bool IsShaking() => nodDetector.shaking;
    bool IsNoding() => nodDetector.noding;
    bool DramaticPausePassed() => _stateMachine.CurrentState is StandingState && timerAfterStanding <= 0;
    bool FinishedTalking() => _stateMachine.CurrentState is TalkingState && (_stateMachine.CurrentState as TalkingState).Finished;

    void At(IState to, IState from, System.Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

}
