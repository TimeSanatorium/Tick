using System;
namespace Tick
{
    public class TwistBreak : ITwistAsync
    {
        public float InsertTime => 0;
        public bool IsCompleted => false;
        private int waitFrame = 0;
        public TwistBreak()
        {
            
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
            if (waitFrame <= 0)
            {
                return false;
            }
            waitFrame--;
            return true;
        }

        public void OnCompleted(Action continuation)
        {
            PlayerLoopHelper.AddTwist(PlayerLoopTiming.EarlyUpdate, this);
        }
    }
}
