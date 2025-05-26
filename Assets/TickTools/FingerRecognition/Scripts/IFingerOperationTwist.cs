using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFingerOperationTwist : ITwistHold
{
    IFingerOperationHandle _FingerOperationHandle { get; set; }
    void HangHook(PlayerLoopTiming playerLoopTiming);
}
