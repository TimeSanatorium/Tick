using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Tick
{
    public class FingerEventHandle : SingleMonoBehaviour<FingerEventHandle>, IFingerOperationHandle
    {
        [SerializeField]
        private float m_pressInterval = 0.2f;
        [SerializeField]
        private SingleFingerDetectionState singleFingerDetectionState;
        [SerializeField]
        private LayerMask m_checkLayerMask;
        [SerializeField]
        private bool m_isFIngerDonw;
        [SerializeField]
        private GraphicRaycaster[] m_graphicRaycasters;
        private EventSystem m_EventSystem;
        public EventSystem EventSystem => m_EventSystem;

        public GraphicRaycaster[] GraphicRaycasters { get => m_graphicRaycasters; }
        public LayerMask CheckLayerMask { get => m_checkLayerMask; set => m_checkLayerMask = value; }
        public SingleFingerDetectionState SingleFingerDetectionState { get => singleFingerDetectionState; set => singleFingerDetectionState = value; }

        private IFingerOperationTwist m_fingerOperationTwist;

        #region Event
        public float PressInterval { get => m_pressInterval; set => m_pressInterval = value; }
        public bool IsFingerDown { get => m_isFIngerDonw; set => m_isFIngerDonw = value; }
        private Action<FingerOperationData> onFingerDown;
        public Action<FingerOperationData> OnFingerDown { get => onFingerDown; set => onFingerDown = value; }
    
        private Action<FingerOperationData> onFingerUp;
        public Action<FingerOperationData> OnFingerUp { get => onFingerUp; set => onFingerUp = value; }
    
        private Action<FingerOperationData> onDoublePress;
        public Action<FingerOperationData> OnDoublePress { get => onDoublePress; set => onDoublePress = value; }

        private Action<FingerOperationData> onPress;
        public Action<FingerOperationData> OnPress { get => onPress; set => onPress = value; }
        private Action<FingerOperationData> onFingerHold;
        public Action<FingerOperationData> OnFingerHold { get => onFingerHold; set => onFingerHold = value; }

        #endregion

        private void Awake()
        {
            m_EventSystem = GameObject.FindObjectOfType<EventSystem>();
#if UNITY_STANDALONE || UNITY_EDITOR
            m_fingerOperationTwist = new MouseTwist(this);
#else
            m_fingerOperationTwist = new SingleFingerTwist(this);
#endif
        }
        /// <summary>
        /// 这里可以获取到所有操作数据信息
        /// </summary>
        /// <param name="fingerOperationDatas">所有的操作数据信息</param>
        public void UpdateInfo(Dictionary<int, FingerOperationData> fingerOperationDatas)
        {
            if (fingerOperationDatas.Count > 0)
            {
                m_isFIngerDonw = fingerOperationDatas[0].IsFingerDown;
            }
        }
        private void OnDestroy()
        {
            m_fingerOperationTwist.Destory();
        }
    }
    public enum SingleFingerDetectionState
    {
        Empty,
        Detection3D,
        Detection2D,
        Both
    }
}