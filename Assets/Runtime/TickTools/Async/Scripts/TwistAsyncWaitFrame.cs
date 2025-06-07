using System;
using UnityEngine;
namespace Tick {
    /// <summary>
    /// µÈ´ýÖ¡½áÊø
    /// </summary>
public class TwistAsyncWaitFrame : ITwistAsync
{
    private int waitFrameCount;
    private float insertTime;
    private Action continuation;
    public float InsertTime => insertTime;
    public bool IsCompleted => false;
    public TwistAsyncWaitFrame(int waitFrameCount)
    {
        this.waitFrameCount = waitFrameCount;
    }
    public ITwistAsync GetAwaiter()
    {
        return this;
    }
    public void GetResult()
    {

    }
    public bool MoveNext()
    {
        waitFrameCount--;
        if (waitFrameCount < 0)
        {
            continuation?.Invoke();
            return false;
        }
        return true;
    }
    public void OnCompleted(Action continuation)
    {
        insertTime = Time.time;
        this.continuation = continuation;
        PlayerLoopHelper.AddTwist(PlayerLoopTiming.Update, this);
    }
}

}