using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangHook : MonoBehaviour
{
    void Start()
    {

#if UNITY_2020_2_OR_NEWER
        TwistHold holdTwist_1 = new TwistHold(PlayerLoopTiming.TimeUpdate);
        holdTwist_1.OnUpdate += () => { Debug.Log("PlayerLoopTiming.TimeUpdate"); };
        TwistHold holdTwist_2 = new TwistHold(PlayerLoopTiming.LastTimeUpdate);
        holdTwist_2.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastTimeUpdate"); };
#endif

        TwistHold holdTwist_3 = new TwistHold(PlayerLoopTiming.Initialization);
        holdTwist_3.OnUpdate += () => { Debug.Log("PlayerLoopTiming.Initialization"); };
        TwistHold holdTwist_4 = new TwistHold(PlayerLoopTiming.LastInitialization);
        holdTwist_4.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastInitialization"); };
        TwistHold holdTwist_5 = new TwistHold(PlayerLoopTiming.FixedUpdate);
        holdTwist_5.OnUpdate += () => { Debug.Log("PlayerLoopTiming.FixedUpdate"); };
        TwistHold holdTwist_6 = new TwistHold(PlayerLoopTiming.LastFixedUpdate);
        holdTwist_6.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastFixedUpdate"); };
        TwistHold holdTwist_7 = new TwistHold(PlayerLoopTiming.PreUpdate);
        holdTwist_7.OnUpdate += () => { Debug.Log("PlayerLoopTiming.PreUpdate"); };
        TwistHold holdTwist_8 = new TwistHold(PlayerLoopTiming.LastPreUpdate);
        holdTwist_8.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastPreUpdate"); };
        TwistHold holdTwist_9 = new TwistHold(PlayerLoopTiming.Update);
        holdTwist_9.OnUpdate += () => { Debug.Log("PlayerLoopTiming.Update"); };
        TwistHold holdTwist_10 = new TwistHold(PlayerLoopTiming.LastUpdate);
        holdTwist_10.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastUpdate"); };
        TwistHold holdTwist_11 = new TwistHold(PlayerLoopTiming.PreLateUpdate);
        holdTwist_11.OnUpdate += () => { Debug.Log("PlayerLoopTiming.PreLateUpdate"); };
        TwistHold holdTwist_12 = new TwistHold(PlayerLoopTiming.LastPreLateUpdate);
        holdTwist_12.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastPreLateUpdate"); };
        TwistHold holdTwist_13 = new TwistHold(PlayerLoopTiming.PostLateUpdate);
        holdTwist_13.OnUpdate += () => { Debug.Log("PlayerLoopTiming.PostLateUpdate"); };
        TwistHold holdTwist_14 = new TwistHold(PlayerLoopTiming.LastPostLateUpdate);
        holdTwist_14.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastPostLateUpdate"); };
    }
}
