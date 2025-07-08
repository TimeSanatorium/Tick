using System.Collections;
using UnityEngine;

public class FingerOperationData
{
    public int fingerId;
    public float downTime;
    public float prePressTime;
    public bool IsFingerDown;
    public Vector2 FingerScreenDownPos;
    public Vector2 FingerScreenUpPos;
    public Vector2 FingerScreenCurrenPos;
    public Vector2 FingerScreenOffset;
    public GameObject CheckCurrentDown;//��ǰ��ָ����λ�ü��Ķ���
    public GameObject CheckCurrentHold;//��ǰ��ָλ�ü��Ķ���
    public GameObject DownUI;//��ǰ��ָ����ȥ��UI
    public FingerOperationData(int fingerId)
    {
        this.fingerId = fingerId;
    }
    public void ResetInfo()
    {
        IsFingerDown = false;
        FingerScreenDownPos = Vector2.zero;
        FingerScreenCurrenPos = Vector2.zero;
        FingerScreenOffset = Vector2.zero;
        CheckCurrentDown = null;
        CheckCurrentHold = null;
    }
}
