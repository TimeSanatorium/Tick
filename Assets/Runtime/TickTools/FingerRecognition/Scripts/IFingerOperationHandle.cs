using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Tick
{
    public interface IFingerOperationHandle
    {
        float PressInterval { get; set; }
        GraphicRaycaster[] GraphicRaycasters { get; }
        EventSystem EventSystem { get; }
        LayerMask CheckLayerMask { get; set; }
        SingleFingerDetectionState SingleFingerDetectionState { get; set; }
        Action<FingerOperationData> OnFingerDown { get; set; }
        Action<FingerOperationData> OnFingerUp { get; set; }
        Action<FingerOperationData> OnDoublePress { get; set; }
        Action<FingerOperationData> OnPress { get; set; }
        Action<FingerOperationData> OnFingerHold { get; set; }
        void UpdateInfo(Dictionary<int, FingerOperationData> OperationDatas);
    }
}