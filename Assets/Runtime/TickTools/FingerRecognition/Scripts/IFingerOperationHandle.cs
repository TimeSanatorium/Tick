using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public interface IFingerOperationHandle
{
    float DownTime { get; set; }
    float PressInterval { get; set; }
    float PrePressTime { get; set; }
    bool IsFingerDown { get; set; }
    Vector2 FingerScreenDownPos { get; set; }
    Vector2 FingerScreenUpPos { get; set; }
    Vector2 FingerScreenCurrenPos { get; set; }
    Vector2 FingerScreenOffset { get; set; }
    Action OnFingerDown { get; set; }
    Action OnFingerUp { get; set; }
    Action OnDoublePress { get; set; }
    Action OnPress { get; set; }
    Action OnFingerHode { get; set; }
    void ResetInfo();
}
