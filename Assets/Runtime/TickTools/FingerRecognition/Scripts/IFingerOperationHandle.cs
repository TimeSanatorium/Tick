using System;
using UnityEngine;
namespace Tick
{
    public interface IFingerOperationHandle
    {
        float PressInterval { get; set; }
        LayerMask CheckLayerMask { get; set; }
        SingleFingerDetectionState SingleFingerDetectionState { get; set; }
        GameObject CheckCurrentDown { get; set; }//当前手指按下位置检测的对象
        GameObject CheckCurrentHold { get; set; }//当前手指位置检测的对象
        bool IsFingerDown { get; set; }
        Vector2 FingerScreenDownPos { get; set; }
        Vector2 FingerScreenUpPos { get; set; }
        Vector2 FingerScreenCurrenPos { get; set; }
        Vector2 FingerScreenOffset { get; set; }
        Action<GameObject> OnFingerDown { get; set; }
        Action<GameObject> OnFingerUp { get; set; }
        Action<GameObject> OnDoublePress { get; set; }
        Action<GameObject> OnPress { get; set; }
        Action<GameObject> OnFingerHode { get; set; }
        void ResetInfo();
    }
}