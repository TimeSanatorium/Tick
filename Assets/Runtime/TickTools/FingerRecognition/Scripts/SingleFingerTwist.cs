using System;
using System.Collections.Generic;
using UnityEngine;
namespace Tick
{

    /// <summary>
    /// 手指钩子
    /// </summary>
    class SingleFingerTwist : IFingerOperationTwist
    {
        private Dictionary<int, float> m_DownTime;
        private Dictionary<int, float> m_PressTime;
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

        public SingleFingerTwist(IFingerOperationHandle fingerEventHandle,Action onStart = null,Action onUpdate = null,Action onCompleted = null)
        {
            this.OnStart = onStart;
            this.OnUpdate = onUpdate;
            this.OnCompleted = OnCompleted;
            this.onUpdate += UpdateFingerOperationHandle;
            this.m_fingerOperationHandle = fingerEventHandle;
            m_DownTime = new Dictionary<int, float>();
            m_PressTime = new Dictionary<int, float>();
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

            }
        }

        private void OnFingerDown(Touch touch)
        {
            if (touch.fingerId == 0)
            {
                m_fingerOperationHandle.IsFingerDown = true;
                m_fingerOperationHandle.FingerScreenDownPos = touch.position;
                m_fingerOperationHandle.FingerScreenCurrenPos = touch.position;
                m_fingerOperationHandle.FingerScreenOffset = Vector2.zero;
                m_fingerOperationHandle.FingerScreenUpPos = Vector2.zero;
            }
            if (!m_DownTime.ContainsKey(touch.fingerId))
            {
                m_DownTime.Add(touch.fingerId, Time.time);
            }
            else
            {
                m_DownTime[touch.fingerId] = Time.time;
            }
            switch (m_fingerOperationHandle.SingleFingerDetectionState)
            {
                case SingleFingerDetectionState.Empty:
                    break;
                case SingleFingerDetectionState.Detection3D:
                    m_fingerOperationHandle.CheckCurrentDown = RayCheckGameObject3D(m_fingerOperationHandle.FingerScreenDownPos);
                    break;
                case SingleFingerDetectionState.Detection2D:
                    m_fingerOperationHandle.CheckCurrentDown = RayCheckGameObject2D(m_fingerOperationHandle.FingerScreenDownPos);
                    break;
                case SingleFingerDetectionState.Both:
                    break;
                default:
                    break;
            }

            m_fingerOperationHandle.OnFingerDown?.Invoke(m_fingerOperationHandle.CheckCurrentDown);
        }

        private void OnFingerMove(Touch touch)
        {
            if (touch.fingerId == 0)
            {
                m_fingerOperationHandle.FingerScreenOffset = touch.position - m_fingerOperationHandle.FingerScreenCurrenPos;
                m_fingerOperationHandle.FingerScreenCurrenPos = touch.position;
            }

            switch (m_fingerOperationHandle.SingleFingerDetectionState)
            {
                case SingleFingerDetectionState.Empty:
                    break;
                case SingleFingerDetectionState.Detection3D:
                    m_fingerOperationHandle.CheckCurrentHold = RayCheckGameObject3D(m_fingerOperationHandle.FingerScreenCurrenPos);
                    break;
                case SingleFingerDetectionState.Detection2D:
                    m_fingerOperationHandle.CheckCurrentHold = RayCheckGameObject2D(m_fingerOperationHandle.FingerScreenCurrenPos);
                    break;
                case SingleFingerDetectionState.Both:
                    break;
                default:
                    break;
            }

            m_fingerOperationHandle.OnFingerHode?.Invoke(m_fingerOperationHandle.CheckCurrentHold);
        }
        private void OnFingerStationary(Touch touch)
        {
            if (touch.fingerId == 0)
            {
                m_fingerOperationHandle.FingerScreenOffset = Vector2.zero;
            }

            m_fingerOperationHandle.OnFingerHode?.Invoke(m_fingerOperationHandle.CheckCurrentHold);
        }
        private void OnFingerUp(Touch touch)
        {
            if (touch.fingerId == 0)
            {
                m_fingerOperationHandle.IsFingerDown = false;
                m_fingerOperationHandle.FingerScreenUpPos = touch.position;
                m_fingerOperationHandle.ResetInfo();
            }

            m_fingerOperationHandle.OnFingerUp?.Invoke(m_fingerOperationHandle.CheckCurrentHold);

            if ((Time.time - m_DownTime[touch.fingerId]) < m_fingerOperationHandle.PressInterval)
            {
                OnPress(touch);
            }
        }
        private void OnPress(Touch touch)
        {
            if (!m_PressTime.ContainsKey(touch.fingerId))
            {
                m_PressTime.Add(touch.fingerId, -1);
            }
            if ((Time.time - m_PressTime[touch.fingerId]) < m_fingerOperationHandle.PressInterval)
            {
                OnFingerDoublePress(touch);
            }
            else
            {
                m_PressTime[touch.fingerId] = Time.time;
                m_fingerOperationHandle.OnPress?.Invoke(m_fingerOperationHandle.CheckCurrentDown);
            }
        }
        private void OnFingerDoublePress(Touch touch)
        {
            if (!m_PressTime.ContainsKey(touch.fingerId))
            {
                m_PressTime.Add(touch.fingerId, 0);
            }
            else
            {
                m_PressTime[touch.fingerId] = 0;
            }
            m_fingerOperationHandle.OnDoublePress?.Invoke(m_fingerOperationHandle.CheckCurrentDown);
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
        #endregion
    }
}