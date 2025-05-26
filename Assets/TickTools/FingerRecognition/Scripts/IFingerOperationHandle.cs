using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFingerOperationHandle
{
    float PressInterval { get; set; }
    bool IsFingerDown { get; set; }
    Vector2 FingerScreenDownPos { get; set; }
    Vector2 FingerScreenUpPos { get; set; }
    Vector2 FingerScreenCurrenPos { get; set; }
    Vector2 FingerScreenOffset { get; set; }
}
