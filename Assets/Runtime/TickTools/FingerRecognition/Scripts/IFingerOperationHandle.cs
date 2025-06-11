using System;
using System.Collections.Generic;
using UnityEngine;
namespace Tick
{
    public interface IFingerOperationHandle
    {
        float PressInterval { get; set; }
        LayerMask CheckLayerMask { get; set; }
        SingleFingerDetectionState SingleFingerDetectionState { get; set; }
        Action<GameObject> OnFingerDown { get; set; }
        Action<GameObject> OnFingerUp { get; set; }
        Action<GameObject> OnDoublePress { get; set; }
        Action<GameObject> OnPress { get; set; }
        Action<GameObject> OnFingerHode { get; set; }
        void UpdateInfo(Dictionary<int, FingerOperationData> OperationDatas);
    }
}