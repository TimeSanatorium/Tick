using System;
using UnityEngine;
namespace Tick 
{ 
    public class TwistAsyncWaitSecond : ITwistAsync
    {
        private float waitTime;
        private Action continuation;
        private float insertTime;
        public float InsertTime => insertTime;
        public bool IsCompleted => false;
        public ITwistAsync GetAwaiter() { return this; }
        public TwistAsyncWaitSecond(float waitTime)
        {
            this.waitTime = waitTime;
        }
        public void GetResult() { }
        public bool MoveNext()
        {
            if (Time.time - insertTime >= waitTime)
            {
                continuation?.Invoke();
                return false;
            }
            return true;
        }
        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            insertTime = Time.time;
            PlayerLoopHelper.AddTwist(PlayerLoopTiming.Update,this);
        }
    }
}