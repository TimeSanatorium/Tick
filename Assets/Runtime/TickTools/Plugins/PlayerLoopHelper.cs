using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
namespace Tick
{

    public class PlayerLoopHelper : MonoBehaviour
    {
        private static TickRunner[] runners;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
    #if UNITY_EDITOR
                EditorApplication.playModeStateChanged += OnPlayModeChanged;
    #endif

    #if UNITY_2020_2_OR_NEWER
            runners = new TickRunner[16];
    #else
            runners = new TickRunner[14];
    #endif
            PlayerLoopSystem playerLoopSystemRoot = PlayerLoop.GetCurrentPlayerLoop();

            InsertLoop(ref playerLoopSystemRoot, typeof(Initialization), PlayerLoopTiming.Initialization, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(EarlyUpdate), PlayerLoopTiming.EarlyUpdate, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(FixedUpdate), PlayerLoopTiming.FixedUpdate, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(PreUpdate), PlayerLoopTiming.PreUpdate, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(Update), PlayerLoopTiming.Update, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(PreLateUpdate), PlayerLoopTiming.PreLateUpdate, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(PostLateUpdate), PlayerLoopTiming.PostLateUpdate, true);

            InsertLoop(ref playerLoopSystemRoot, typeof(Update), PlayerLoopTiming.LastUpdate, false);
            InsertLoop(ref playerLoopSystemRoot, typeof(Initialization), PlayerLoopTiming.LastInitialization, false);
            InsertLoop(ref playerLoopSystemRoot, typeof(EarlyUpdate), PlayerLoopTiming.LastEarlyUpdate, false);
            InsertLoop(ref playerLoopSystemRoot, typeof(FixedUpdate), PlayerLoopTiming.LastFixedUpdate, false);
            InsertLoop(ref playerLoopSystemRoot, typeof(PreUpdate), PlayerLoopTiming.LastPreUpdate, false);
            InsertLoop(ref playerLoopSystemRoot, typeof(PreLateUpdate), PlayerLoopTiming.LastPreLateUpdate, false);
            InsertLoop(ref playerLoopSystemRoot, typeof(PostLateUpdate), PlayerLoopTiming.LastPostLateUpdate, false);

    #if UNITY_2020_2_OR_NEWER
            InsertLoop(ref playerLoopSystemRoot, typeof(TimeUpdate), PlayerLoopTiming.TimeUpdate, true);
            InsertLoop(ref playerLoopSystemRoot, typeof(TimeUpdate), PlayerLoopTiming.LastTimeUpdate, false);
    #endif
            PlayerLoop.SetPlayerLoop(playerLoopSystemRoot);

        }

        private static void InsertLoop(ref PlayerLoopSystem root, Type insertTimming, PlayerLoopTiming playerLoopTiming, bool isFirst)
        {
            PlayerLoopSystem[] subSystemList = root.subSystemList;
            int index = FindLoopSystemIndex(subSystemList, insertTimming);
            root.subSystemList[index].subSystemList = InsertRunner(subSystemList[index].subSystemList, insertTimming, playerLoopTiming, isFirst);
        }

        private static PlayerLoopSystem[] InsertRunner(PlayerLoopSystem[] oriSublist, Type insertTimming, PlayerLoopTiming playerLoopTiming, bool isFirst)
        {
            PlayerLoopSystem[] result = new PlayerLoopSystem[oriSublist.Length + 1];
            TickRunner tickRunner = new TickRunner(playerLoopTiming);
            PlayerLoopSystem InsertPlayerLoop = new PlayerLoopSystem
            {
                type = insertTimming,
                updateDelegate = tickRunner.Run,
            };
            if (isFirst)
            {
                result[0] = InsertPlayerLoop;
                Array.Copy(oriSublist, 0, result, 1, oriSublist.Length);
            }
            else
            {
                Array.Copy(oriSublist, 0, result, 0, oriSublist.Length);
                result[result.Length - 1] = InsertPlayerLoop;
            }
            if (runners[(int)playerLoopTiming] == null)
            {
                runners[(int)playerLoopTiming] = tickRunner;
            }
            else
            {
                throw new Exception("Hook repeat insertion please check:" + playerLoopTiming.ToString());
            }
            return result;
        }

        private static int FindLoopSystemIndex(PlayerLoopSystem[] rootSubSystemList, Type systemType)
        {
            int result = -1;
            for (int i = 0; i < rootSubSystemList.Length; i++)
            {
                PlayerLoopSystem playerLoopSystem = rootSubSystemList[i];
                if (playerLoopSystem.type == systemType)
                {
                    result = i;
                    break;
                }
            }
            if (result < 0)
            {
                throw new Exception("Type not found, add the appropriate type to the root object's list:" + systemType.FullName);
            }
            return result;
        }

        public static void AddTwist(PlayerLoopTiming playerLoopTiming, ITwist twist)
        {
            if (runners[(int)playerLoopTiming] == null)
            {
                throw new Exception($"Execution time has not been initialized Please check initialization : {playerLoopTiming}");
            }
            TickRunner tickRunner = runners[(int)playerLoopTiming];
            tickRunner.AddTwist(twist);
        }
#if UNITY_EDITOR
        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.ExitingPlayMode:
                    PlayerLoopSystem playerLoopSystem = PlayerLoop.GetDefaultPlayerLoop();
                    PlayerLoop.SetPlayerLoop(playerLoopSystem);
                    EditorApplication.playModeStateChanged -= OnPlayModeChanged;
                    break;
                default:
                    break;
            }
        }
#endif
    }
}