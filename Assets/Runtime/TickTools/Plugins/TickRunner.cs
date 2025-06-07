using System;
using UnityEngine;
namespace Tick
{
    internal sealed class TickRunner
    {
        public PlayerLoopTiming playerLoopTiming;
        private ITwist[] twists;
        private int tail;//永远指向下一个为空的数据
        public TickRunner(PlayerLoopTiming playerLoopTiming)
        {
            this.playerLoopTiming = playerLoopTiming;
            twists = new ITwist[16];
            tail = 0;
        }
        public void Run()
        {
            if(playerLoopTiming == PlayerLoopTiming.Update)
            if (tail >= twists.Length)
                RecycleTwistArray();
            for (int i = 0; i < tail; i++)
            {
                ITwist twist = twists[i];
                if (twist == null) { continue; }
                if (!twist.MoveNext())
                {
                    twists[i] = null;
                }
            }
        }


        public void AddTwist(ITwist twist)
        {
            if (tail >= twists.Length)
            {
                ExpandTwistArray();
            }
            twists[tail] = twist;
            tail++;
        }

        private void RecycleTwistArray()
        {
            int index = -1;
            for (int i = 0; i < tail; i++)
            {
                ITwist twist = twists[i];
                if (index == -1 && twist == null)
                {
                    index = i;
                }
                if (index != -1 && twist != null)
                {
                    twists[index] = twist;
                    twists[i] = null;
                    index++;
                }
            }
            if (index != -1)
            {
                tail = index;
            }
        }
        private void ExpandTwistArray()
        {
            Array.Resize(ref twists, checked(tail * 2));
        }

        #region TestFunction
        public void TestShowAllTwists()
        {
            for (int i = 0; i < tail; i++)
            {
                ITwist twist = twists[i];
                Debug.Log(twist);
            }
        }
        #endregion
    }
    public enum PlayerLoopTiming
    {
        Initialization = 0,
        LastInitialization = 1,

        EarlyUpdate = 2,
        LastEarlyUpdate = 3,

        FixedUpdate = 4,
        LastFixedUpdate = 5,

        PreUpdate = 6,
        LastPreUpdate = 7,

        Update = 8,
        LastUpdate = 9,

        PreLateUpdate = 10,
        LastPreLateUpdate = 11,

        PostLateUpdate = 12,
        LastPostLateUpdate = 13,

    #if UNITY_2020_2_OR_NEWER
        TimeUpdate = 14,
        LastTimeUpdate = 15,
    #endif
    }
}