using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {

#if UNITY_2020_2_OR_NEWER
        HoldTwist holdTwist_1 = new HoldTwist(PlayerLoopTiming.TimeUpdate);
        holdTwist_1.OnUpdate += () => { Debug.Log("PlayerLoopTiming.TimeUpdate"); };
        HoldTwist holdTwist_2 = new HoldTwist(PlayerLoopTiming.LastTimeUpdate);
        holdTwist_2.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastTimeUpdate"); };
#endif

        HoldTwist holdTwist_3 = new HoldTwist(PlayerLoopTiming.Initialization);
        holdTwist_3.OnUpdate += () => { Debug.Log("PlayerLoopTiming.Initialization"); };
        HoldTwist holdTwist_4 = new HoldTwist(PlayerLoopTiming.LastInitialization);
        holdTwist_4.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastInitialization"); };
        HoldTwist holdTwist_5 = new HoldTwist(PlayerLoopTiming.FixedUpdate);
        holdTwist_5.OnUpdate += () => { Debug.Log("PlayerLoopTiming.FixedUpdate"); };
        HoldTwist holdTwist_6 = new HoldTwist(PlayerLoopTiming.LastFixedUpdate);
        holdTwist_6.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastFixedUpdate"); };
        HoldTwist holdTwist_7 = new HoldTwist(PlayerLoopTiming.PreUpdate);
        holdTwist_7.OnUpdate += () => { Debug.Log("PlayerLoopTiming.PreUpdate"); };
        HoldTwist holdTwist_8 = new HoldTwist(PlayerLoopTiming.LastPreUpdate);
        holdTwist_8.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastPreUpdate"); };
        HoldTwist holdTwist_9 = new HoldTwist(PlayerLoopTiming.Update);
        holdTwist_9.OnUpdate += () => { Debug.Log("PlayerLoopTiming.Update"); };
        HoldTwist holdTwist_10 = new HoldTwist(PlayerLoopTiming.LastUpdate);
        holdTwist_10.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastUpdate"); };
        HoldTwist holdTwist_11 = new HoldTwist(PlayerLoopTiming.PreLateUpdate);
        holdTwist_11.OnUpdate += () => { Debug.Log("PlayerLoopTiming.PreLateUpdate"); };
        HoldTwist holdTwist_12 = new HoldTwist(PlayerLoopTiming.LastPreLateUpdate);
        holdTwist_12.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastPreLateUpdate"); };
        HoldTwist holdTwist_13 = new HoldTwist(PlayerLoopTiming.PostLateUpdate);
        holdTwist_13.OnUpdate += () => { Debug.Log("PlayerLoopTiming.PostLateUpdate"); };
        HoldTwist holdTwist_14 = new HoldTwist(PlayerLoopTiming.LastPostLateUpdate);
        holdTwist_14.OnUpdate += () => { Debug.Log("PlayerLoopTiming.LastPostLateUpdate"); };
    }
}
