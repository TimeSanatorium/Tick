using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float PressInterval { get => m_pressInterval; set => m_pressInterval = value; }
    public Vector2 FingerScreenDownPos { get => m_fingerScreenDownPos; set { m_fingerScreenDownPos = value; } }
    public Vector2 FingerScreenUpPos { get => m_fingerScreenUpPos;  set { m_fingerScreenUpPos = value; } }
    public Vector2 FingerScreenCurrenPos { get => m_fingerScreenCurrenPos; set { m_fingerScreenCurrenPos = value; } }
    public Vector2 FingerScreenOffset { get => m_fingerScreenOffset; set { m_fingerScreenOffset = value; } }

    public bool IsFingerDown { get => m_isFIngerDonw; set => m_isFIngerDonw = value; }

    private void Awake()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        new MouseTwist(this);
#else
        new FingerTwist(this);
#endif
    }
}
