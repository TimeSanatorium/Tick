using System;
using UnityEngine;

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

    private void OnMouseLeftDown()
    {
        m_fingerOperationHandle.IsFingerDown = true;
        m_fingerOperationHandle.FingerScreenDownPos = Input.mousePosition;
        m_fingerOperationHandle.FingerScreenCurrenPos = Input.mousePosition;
        m_fingerOperationHandle.FingerScreenOffset = Vector2.zero;
        m_fingerOperationHandle.DownTime = Time.time;
        m_fingerOperationHandle.FingerScreenUpPos = Vector2.zero;

        m_fingerOperationHandle.OnFingerDown?.Invoke();
    }
    private void OnMouserLeftMove()
    {
        m_fingerOperationHandle.FingerScreenOffset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - m_fingerOperationHandle.FingerScreenCurrenPos;
        m_fingerOperationHandle.FingerScreenCurrenPos = Input.mousePosition;

        m_fingerOperationHandle.OnFingerHode?.Invoke();
    }
    private void OnMouseLeftUp()
    {
        m_fingerOperationHandle.IsFingerDown = false;
        m_fingerOperationHandle.ResetInfo();
        m_fingerOperationHandle.FingerScreenUpPos = Input.mousePosition;

        m_fingerOperationHandle.OnFingerUp?.Invoke();

        if ((Time.time - m_fingerOperationHandle.DownTime) < m_fingerOperationHandle.PressInterval)
        {
            OnMouseLeftPress();
        }
    }

    private void OnMouseLeftPress()
    {

        if ((Time.time - m_fingerOperationHandle.PrePressTime) < m_fingerOperationHandle.PressInterval)
        {
            OnMouseDoublePress();
        }
        else
        {
            m_fingerOperationHandle.OnPress?.Invoke();
            m_fingerOperationHandle.PrePressTime = Time.time;
        }
    }

    private void OnMouseDoublePress()
    {
        m_fingerOperationHandle.PrePressTime = 0;
        m_fingerOperationHandle.OnDoublePress?.Invoke();
    }
    #region Operation

    #endregion
}