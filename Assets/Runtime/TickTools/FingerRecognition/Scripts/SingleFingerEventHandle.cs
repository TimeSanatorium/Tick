using System;
using UnityEngine;
namespace Tick{

public class SingleFingerEventHandle : SingleMonoBehaviour<SingleFingerEventHandle>, IFingerOperationHandle
{
    [SerializeField]
    private float m_pressInterval;
    [SerializeField]
    private Vector2 m_fingerScreenDownPos;
    [SerializeField]
    private Vector2 m_fingerScreenUpPos;
    [SerializeField]
    private Vector2 m_fingerScreenCurrenPos;
    [SerializeField]
    private Vector2 m_fingerScreenOffset;
    [SerializeField]
    private bool m_isFIngerDonw;
    private float donwTime;
    private float prePressTime;
    public float PrePressTime { get => prePressTime; set => prePressTime = value; }
    public float PressInterval { get => m_pressInterval; set => m_pressInterval = value; }
    public Vector2 FingerScreenDownPos { get => m_fingerScreenDownPos; set => m_fingerScreenDownPos = value; }
    public Vector2 FingerScreenUpPos { get => m_fingerScreenUpPos;  set => m_fingerScreenUpPos = value; } 
    public Vector2 FingerScreenCurrenPos { get => m_fingerScreenCurrenPos; set => m_fingerScreenCurrenPos = value;  }
    public Vector2 FingerScreenOffset { get => m_fingerScreenOffset; set => m_fingerScreenOffset = value; }
    public bool IsFingerDown { get => m_isFIngerDonw; set => m_isFIngerDonw = value; }
    public float DownTime { get => donwTime; set => donwTime = value; }
    
    private Action onFingerDown;
    public Action OnFingerDown { get => onFingerDown; set => onFingerDown = value; }
    
    private Action onFingerUp;
    public Action OnFingerUp { get => onFingerUp; set => onFingerUp = value; }
    
    private Action onDoublePress;
    public Action OnDoublePress { get => onDoublePress; set => onDoublePress = value; }

    private Action onPress;
    public Action OnPress { get => onPress; set => onPress = value; }
    private Action onFingerHode;
    public Action OnFingerHode { get => onFingerHode; set => onFingerHode = value; }

    private void Awake()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        //new MouseTwist(this);
        new SingleFingerTwist(this);
#else
        new SingleFingerTwist(this);
#endif
        TestHookEvent();
    }
    public void ResetInfo()
    {
        FingerScreenDownPos = Vector2.zero;
        FingerScreenCurrenPos = Vector2.zero;
        FingerScreenOffset = Vector2.zero;
    }

    private void TestHookEvent()
    {
        OnFingerDown += () => { Debug.Log("按下"); };
        OnFingerUp += () => { Debug.Log("抬起"); };
        OnPress += () => { Debug.Log("点击"); };
        OnDoublePress += () => { Debug.Log("单击"); };
    }
}

}