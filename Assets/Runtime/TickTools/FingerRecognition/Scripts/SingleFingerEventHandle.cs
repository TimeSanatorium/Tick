using System;
using System.Collections.Generic;
using UnityEngine;
namespace Tick
{
    public class SingleFingerEventHandle : SingleMonoBehaviour<SingleFingerEventHandle>, IFingerOperationHandle
    {
        public int MonitoringID;
        [SerializeField]
        private float m_pressInterval = 0.2f;
        [SerializeField]
        private Vector2 m_fingerScreenDownPos;
        [SerializeField]
        private Vector2 m_fingerScreenUpPos;
        [SerializeField]
        private Vector2 m_fingerScreenCurrenPos;
        [SerializeField]
        private Vector2 m_fingerScreenOffset;
        [SerializeField]
        private SingleFingerDetectionState singleFingerDetectionState;
        [SerializeField]
        private LayerMask m_checkLayerMask;
        [SerializeField]
        private bool m_isFIngerDonw;
        public LayerMask CheckLayerMask { get => m_checkLayerMask; set => m_checkLayerMask = value; }
        public SingleFingerDetectionState SingleFingerDetectionState { get => singleFingerDetectionState; set => singleFingerDetectionState = value; }

        private Dictionary<int, GameObject> m_CheckCurrentDown = new Dictionary<int, GameObject>();//手指按下去检测到的对象
        private Dictionary<int, GameObject> m_CheckCurrentHold = new Dictionary<int, GameObject>();//手指当前位置下面检测到的对象

        #region Event
        public float PressInterval { get => m_pressInterval; set => m_pressInterval = value; }
        public Vector2 FingerScreenDownPos { get => m_fingerScreenDownPos; set => m_fingerScreenDownPos = value; }
        public Vector2 FingerScreenUpPos { get => m_fingerScreenUpPos;  set => m_fingerScreenUpPos = value; } 
        public Vector2 FingerScreenCurrenPos { get => m_fingerScreenCurrenPos; set => m_fingerScreenCurrenPos = value;  }
        public Vector2 FingerScreenOffset { get => m_fingerScreenOffset; set => m_fingerScreenOffset = value; }
        public bool IsFingerDown { get => m_isFIngerDonw; set => m_isFIngerDonw = value; }
        private Action<GameObject> onFingerDown;
        public Action<GameObject> OnFingerDown { get => onFingerDown; set => onFingerDown = value; }
    
        private Action<GameObject> onFingerUp;
        public Action<GameObject> OnFingerUp { get => onFingerUp; set => onFingerUp = value; }
    
        private Action<GameObject> onDoublePress;
        public Action<GameObject> OnDoublePress { get => onDoublePress; set => onDoublePress = value; }

        private Action<GameObject> onPress;
        public Action<GameObject> OnPress { get => onPress; set => onPress = value; }
        private Action<GameObject> onFingerHode;
        public Action<GameObject> OnFingerHode { get => onFingerHode; set => onFingerHode = value; }
        #endregion

        private void Awake()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            new MouseTwist(this);
#else
            new SingleFingerTwist(this);
#endif
        }
        public void UpdateInfo(Dictionary<int, FingerOperationData> fingerOperationDatas)
        {
            FingerOperationData data;

            if (fingerOperationDatas.TryGetValue(MonitoringID,out data))
            {
                IsFingerDown = data.IsFingerDown;
                m_fingerScreenDownPos = data.FingerScreenDownPos;
                m_fingerScreenCurrenPos = data.FingerScreenCurrenPos;
                m_fingerScreenUpPos = data.FingerScreenUpPos;
                m_fingerScreenOffset = data.FingerScreenOffset;
            }
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