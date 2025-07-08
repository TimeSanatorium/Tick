using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Tick
{
    /// <summary>
    /// 手指钩子
    /// </summary>
    class SingleFingerTwist : IFingerOperationTwist
    {
        private bool isHold = true;
        public bool IsHold { get => isHold; set => isHold = value; }

        #region TwistLifeCycle
        private Action onUpdate;
        public Action OnUpdate { get => onUpdate; set => onUpdate += value; }
        private Action onStart;
        public Action OnStart { get => onStart; set => onStart += value; }
        private Action onCompleted;
        public Action OnCompleted { get => onCompleted; set => onCompleted += value; }
        #endregion
        
        private IFingerOperationHandle m_fingerOperationHandle;
        public IFingerOperationHandle _FingerOperationHandle { get => m_fingerOperationHandle; set => m_fingerOperationHandle = value; }
        private Dictionary<int, FingerOperationData> m_FingerOperationDatas;
        public SingleFingerTwist(IFingerOperationHandle fingerEventHandle, Action onStart = null, Action onUpdate = null, Action onCompleted = null)
        {
            this.OnStart = onStart;
            this.OnUpdate = onUpdate;
            this.OnCompleted = OnCompleted;
            this.onUpdate += UpdateFingerOperationHandle;
            this.m_fingerOperationHandle = fingerEventHandle;
            m_FingerOperationDatas = new Dictionary<int, FingerOperationData>();
            HangHook(PlayerLoopTiming.LastEarlyUpdate);
        }
        public void HangHook(PlayerLoopTiming playerLoopTiming)
        {
            PlayerLoopHelper.AddTwist(playerLoopTiming, this);
        }
        public bool MoveNext()
        {
            OnUpdate?.Invoke();
            if (!IsHold)
                onCompleted?.Invoke();
            return IsHold;
        }
        private void UpdateFingerOperationHandle()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            OnFingerDown(touch);
                            break;
                        case TouchPhase.Moved:
                            OnFingerMove(touch);
                            break;
                        case TouchPhase.Stationary:
                            OnFingerStationary(touch);
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            OnFingerUp(touch);
                            break;
                        default:
                            break;
                    }
                }
                _FingerOperationHandle.UpdateInfo(m_FingerOperationDatas);
            }
        }
        private void OnFingerDown(Touch touch)
        {
            if (!m_FingerOperationDatas.ContainsKey(touch.fingerId))
            {
                m_FingerOperationDatas.Add(touch.fingerId, new FingerOperationData(touch.fingerId));
            }

            FingerOperationData data = m_FingerOperationDatas[touch.fingerId];
            data.IsFingerDown = true;
            data.FingerScreenDownPos = touch.position;
            data.FingerScreenCurrenPos = touch.position;
            data.FingerScreenOffset = Vector2.zero;
            data.FingerScreenUpPos = Vector2.zero;
            data.downTime = Time.time;
            data.DownUI = GetDownUI(touch.position);
            switch (m_fingerOperationHandle.SingleFingerDetectionState)
            {
                case SingleFingerDetectionState.Empty:
                    break;
                case SingleFingerDetectionState.Detection3D:
                    data.CheckCurrentDown = RayCheckGameObject3D(data.FingerScreenDownPos);
                    break;
                case SingleFingerDetectionState.Detection2D:
                    data.CheckCurrentDown = RayCheckGameObject2D(data.FingerScreenDownPos);
                    break;
                case SingleFingerDetectionState.Both:
                    break;
                default:
                    break;
            }

            m_fingerOperationHandle.OnFingerDown?.Invoke(data);
        }
        private void OnFingerMove(Touch touch)
        {
            FingerOperationData data = m_FingerOperationDatas[touch.fingerId];
            
            data.FingerScreenOffset = touch.position - data.FingerScreenCurrenPos;
            data.FingerScreenCurrenPos = touch.position;

            switch (m_fingerOperationHandle.SingleFingerDetectionState)
            {
                case SingleFingerDetectionState.Empty:
                    break;
                case SingleFingerDetectionState.Detection3D:
                    data.CheckCurrentHold = RayCheckGameObject3D(data.FingerScreenCurrenPos);
                    break;
                case SingleFingerDetectionState.Detection2D:
                    data.CheckCurrentHold = RayCheckGameObject2D(data.FingerScreenCurrenPos);
                    break;
                case SingleFingerDetectionState.Both:
                    break;
                default:
                    break;
            }

            m_fingerOperationHandle.OnFingerHold?.Invoke(data);
        }
        private void OnFingerStationary(Touch touch)
        {
            FingerOperationData data = m_FingerOperationDatas[touch.fingerId];
            data.FingerScreenOffset = Vector2.zero;
            m_fingerOperationHandle.OnFingerHold?.Invoke(data);
        }
        private void OnFingerUp(Touch touch)
        {
            FingerOperationData data = m_FingerOperationDatas[touch.fingerId];
            data.IsFingerDown = false;
            data.FingerScreenUpPos = touch.position;

            if ((Time.time - data.downTime) < m_fingerOperationHandle.PressInterval)
            {
                OnPress(touch);
            }

            m_fingerOperationHandle.OnFingerUp?.Invoke(data);
            
            data.ResetInfo();
        }
        private void OnPress(Touch touch)
        {
            FingerOperationData data = m_FingerOperationDatas[touch.fingerId];
            if ((Time.time - data.prePressTime) < m_fingerOperationHandle.PressInterval)
            {
                OnFingerDoublePress(touch);
            }
            m_fingerOperationHandle.OnPress?.Invoke(data);
            data.prePressTime = Time.time;
        }
        private void OnFingerDoublePress(Touch touch)
        {
            FingerOperationData data = m_FingerOperationDatas[touch.fingerId];

            data.prePressTime = 0;
            m_fingerOperationHandle.OnDoublePress?.Invoke(data);
        }
        #region Check
        private GameObject RayCheckGameObject3D(Vector2 screenPosition)
        {
            GameObject result = null;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, m_fingerOperationHandle.CheckLayerMask))
            {
                result = hit.collider.gameObject;
            }
            return result;
        }
        private GameObject RayCheckGameObject2D(Vector2 screenPosition)
        {
            GameObject result = null;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
            if (raycastHit2D.collider != null)
            {
                result = raycastHit2D.collider.gameObject;
            }
            return result;
        }
        private GameObject GetDownUI(Vector2 screenPosition)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            foreach (GraphicRaycaster raycaster in m_fingerOperationHandle.GraphicRaycasters)
            {
                PointerEventData pointerEventData = new PointerEventData(m_fingerOperationHandle.EventSystem);
                pointerEventData.position = screenPosition;
                raycaster.Raycast(pointerEventData, results);
                if (results.Count > 0)
                {
                    return results[0].gameObject;
                }
            }
            return null;
        }
        #endregion
    }
}