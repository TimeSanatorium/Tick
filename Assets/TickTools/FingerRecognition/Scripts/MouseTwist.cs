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
        Debug.Log("鼠标左键按下");
        m_fingerOperationHandle.IsFingerDown = true;
    }
    private void OnMouserLeftMove()
    {
        Debug.Log("鼠标左键移动");
    }
    private void OnMouseLeftUp()
    {
        Debug.Log("鼠标左键抬起");
        m_fingerOperationHandle.IsFingerDown = false;
    }
    #region Operation

    #endregion
}