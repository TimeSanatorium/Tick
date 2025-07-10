using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Tick
{
    /// <summary>
    /// Êó±ê¼üÅÌ¹³×Ó
    /// </summary>
    class MouseTwist : IFingerOperationTwist
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
        
        private Dictionary<int, FingerOperationData> m_fingerOperationDatas;
        private IFingerOperationHandle m_fingerOperationHandle;
        public MouseTwist(IFingerOperationHandle fingerOperationHandle, Action onStart = null, Action onUpdate = null, Action onCompleted = null)
        {
            this.OnStart = onStart;
            this.OnUpdate = onUpdate;
            this.OnCompleted = OnCompleted;
            this.onUpdate += UpdateFingerOperationHandle;
            m_fingerOperationDatas = new Dictionary<int, FingerOperationData>();
            m_fingerOperationHandle = fingerOperationHandle;
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

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
            {
                Debug.LogError("ÔÝÍ£±à¼­Æ÷");
                EditorApplication.isPaused = !EditorApplication.isPaused;
            }
#endif
            m_fingerOperationHandle.UpdateInfo(m_fingerOperationDatas);
        }
        #region Operation
        private void OnMouseLeftDown()
        {
            if (!m_fingerOperationDatas.ContainsKey(0))
            {
                m_fingerOperationDatas.Add(0, new FingerOperationData(0));
            }
            FingerOperationData data = m_fingerOperationDatas[0];

            data.IsFingerDown = true;
            data.FingerScreenDownPos = Input.mousePosition;
            data.FingerScreenCurrenPos = Input.mousePosition;
            data.FingerScreenOffset = Vector2.zero;
            data.downTime= Time.time;
            data.FingerScreenUpPos = Vector2.zero;

            data.DownUI = GetDownUI(Input.mousePosition);

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
        private void OnMouserLeftMove()
        {
            FingerOperationData data = m_fingerOperationDatas[0];

            data.FingerScreenOffset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - data.FingerScreenCurrenPos;
            data.FingerScreenCurrenPos = Input.mousePosition;

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
        private void OnMouseLeftUp()
        {
            FingerOperationData data = m_fingerOperationDatas[0];

            data.IsFingerDown = false;
            data.FingerScreenUpPos = Input.mousePosition;

            if ((Time.time - data.downTime) < m_fingerOperationHandle.PressInterval)
            {
                OnMouseLeftPress();
            }

            m_fingerOperationHandle.OnFingerUp?.Invoke(data);

            data.ResetInfo();
        }
        private void OnMouseLeftPress()
        {
            FingerOperationData data = m_fingerOperationDatas[0];
            m_fingerOperationHandle.OnPress?.Invoke(data);
            data.prePressTime = Time.time;
            if ((Time.time - data.prePressTime) < m_fingerOperationHandle.PressInterval)
            {
                OnMouseDoublePress();
            }
        }
        private void OnMouseDoublePress()
        {
            FingerOperationData data = m_fingerOperationDatas[0];

            data.prePressTime = 0;
            m_fingerOperationHandle.OnDoublePress?.Invoke(data);
        }
        #endregion

        #region Check
        private GameObject RayCheckGameObject3D(Vector2 screenPosition)
        {
            GameObject result = null;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, m_fingerOperationHandle.CheckLayerMask)) 
            {
                result = hit.collider.gameObject;
            }
            return result;
        }
        private GameObject RayCheckGameObject2D(Vector2 screenPosition)
        {
            GameObject result = null;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(mousePos, Vector3.forward,100, m_fingerOperationHandle.CheckLayerMask);
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

        public void Destory()
        {
            isHold = false;
        }
    }
}