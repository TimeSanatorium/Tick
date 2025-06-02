using System;
using Unity.VisualScripting;
using UnityEngine;
namespace Tick{

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

    public SingleFingerTwist(IFingerOperationHandle fingerEventHandle,Action onStart = null,Action onUpdate = null,Action onCompleted = null)
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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
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

    private void OnFingerDown(Touch touch)
    {
        m_fingerOperationHandle.IsFingerDown = true;
        m_fingerOperationHandle.FingerScreenDownPos = touch.position;
        m_fingerOperationHandle.FingerScreenCurrenPos = touch.position;
        m_fingerOperationHandle.FingerScreenOffset = Vector2.zero;
        m_fingerOperationHandle.DownTime = Time.time;
        m_fingerOperationHandle.FingerScreenUpPos = Vector2.zero;

        m_fingerOperationHandle.OnFingerDown?.Invoke();
    }

    private void OnFingerMove(Touch touch)
    {
        m_fingerOperationHandle.FingerScreenOffset = touch.position - m_fingerOperationHandle.FingerScreenCurrenPos;
        m_fingerOperationHandle.FingerScreenCurrenPos = touch.position;

        m_fingerOperationHandle.OnFingerHode?.Invoke();
    }
    private void OnFingerStationary(Touch touch)
    {
        m_fingerOperationHandle.FingerScreenOffset = Vector2.zero;

        m_fingerOperationHandle.OnFingerHode?.Invoke();
    }
    private void OnFingerUp(Touch touch)
    {
        if (Input.touchCount <= 1)
        {
            m_fingerOperationHandle.IsFingerDown = false;
            m_fingerOperationHandle.FingerScreenUpPos = touch.position;
        }
        
        m_fingerOperationHandle.ResetInfo();

        m_fingerOperationHandle.OnFingerUp?.Invoke();

        if ((Time.time - m_fingerOperationHandle.DownTime) < m_fingerOperationHandle.PressInterval)
        {
            OnPress(touch);
        }
    }
    private void OnPress(Touch touch)
    {
        if ((Time.time - m_fingerOperationHandle.PrePressTime) < m_fingerOperationHandle.PressInterval)
        {
            OnFingerDoublePress();
        }
        else
        {
            m_fingerOperationHandle.OnPress?.Invoke();
            m_fingerOperationHandle.PrePressTime = Time.time;
        }
    }
    private void OnFingerDoublePress()
    {
        m_fingerOperationHandle.PrePressTime = 0;
        m_fingerOperationHandle.OnDoublePress?.Invoke();
    }
}
}