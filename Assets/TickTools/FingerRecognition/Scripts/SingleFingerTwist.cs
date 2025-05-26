using System;
using UnityEngine;

/// <summary>
/// 限制只能单指控制
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
        Debug.Log("单手指更新状态");
    }

}