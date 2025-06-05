using System;
using UnityEngine;
namespace Tick {
    /// <summary>
    /// 等待条件完成后执行后续内容
    /// </summary>
public class TwistAsyncWaitCondition : ITwistAsync
{
    private Func<bool> condition;
    private Action continuation;
    private float insertTime;
    private float maxWaitTime;
    public float InsertTime => insertTime;
    public bool IsCompleted => false;
    public TwistAsyncWaitCondition(Func<bool> condition, float maxWaitTime = float.MaxValue)
    {
        this.condition = condition;
        insertTime = Time.time;
        this.maxWaitTime = maxWaitTime > 0 ? maxWaitTime : float.MaxValue;
    }

    public ITwistAsync GetAwaiter()
    {
        return this;
    }
    public void GetResult() { }
    public bool MoveNext()
    {
        if (condition())
        {
            continuation?.Invoke();
            return false;
        }
        if (Time.time - insertTime > maxWaitTime)
        {
            Debug.LogWarning("TwistAsyncWaitCondition: Condition not met within the specified time limit.");
            return false;
        }
        return true;
    }
    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
        PlayerLoopHelper.AddTwist(PlayerLoopTiming.Update, this);
    }
}
}