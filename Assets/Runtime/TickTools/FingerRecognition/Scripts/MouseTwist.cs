using System;
using UnityEngine;
namespace Tick
{
    /// <summary>
    /// Êó±ê¼üÅÌ¹³×Ó
    /// </summary>
    class MouseTwist : IFingerOperationTwist
    {
        private bool isHold = true;
        private float m_downTime;
        private float m_pressTime;
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

        public MouseTwist(IFingerOperationHandle fingerEventHandle, Action onStart = null, Action onUpdate = null, Action onCompleted = null)
        {
            this.OnStart = onStart;
            this.OnUpdate = onUpdate;
            this.OnCompleted = OnCompleted;
            this.onUpdate += UpdateFingerOperationHandle;
            this.m_fingerOperationHandle = fingerEventHandle;

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
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseLeftDown();
            }
            else if (Input.GetMouseButton(0))
            {
                OnMouserLeftMove();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnMouseLeftUp();
            }
        }

        #region Operation
        private void OnMouseLeftDown()
        {
            m_fingerOperationHandle.IsFingerDown = true;
            m_fingerOperationHandle.FingerScreenDownPos = Input.mousePosition;
            m_fingerOperationHandle.FingerScreenCurrenPos = Input.mousePosition;
            m_fingerOperationHandle.FingerScreenOffset = Vector2.zero;
            m_downTime = Time.time;
            m_fingerOperationHandle.FingerScreenUpPos = Vector2.zero;

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
        private void OnMouserLeftMove()
        {
            m_fingerOperationHandle.FingerScreenOffset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - m_fingerOperationHandle.FingerScreenCurrenPos;
            m_fingerOperationHandle.FingerScreenCurrenPos = Input.mousePosition;

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
        private void OnMouseLeftUp()
        {
            m_fingerOperationHandle.IsFingerDown = false;
            m_fingerOperationHandle.ResetInfo();
            m_fingerOperationHandle.FingerScreenUpPos = Input.mousePosition;

            m_fingerOperationHandle.OnFingerUp?.Invoke(m_fingerOperationHandle.CheckCurrentHold);

            if ((Time.time - m_downTime) < m_fingerOperationHandle.PressInterval)
            {
                OnMouseLeftPress();
            }
        }

        private void OnMouseLeftPress()
        {

            if ((Time.time - m_pressTime) < m_fingerOperationHandle.PressInterval)
            {
                OnMouseDoublePress();
            }
            else
            {
                m_fingerOperationHandle.OnPress?.Invoke(m_fingerOperationHandle.CheckCurrentDown);
                m_pressTime = Time.time;
            }
        }

        private void OnMouseDoublePress()
        {
            m_pressTime = 0;
            m_fingerOperationHandle.OnDoublePress?.Invoke(m_fingerOperationHandle.CheckCurrentDown);
        }
        #endregion

        #region Check
        private GameObject RayCheckGameObject3D(Vector2 screenPosition)
        {
            GameObject result = null;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,float.MaxValue, m_fingerOperationHandle.CheckLayerMask))
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