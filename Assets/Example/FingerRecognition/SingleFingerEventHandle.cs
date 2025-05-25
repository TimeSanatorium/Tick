using System;
using UnityEngine;
public class SingleFingerEventHandle : SingleMonoBehaviour<SingleFingerEventHandle>
{
    public int IsLimitingId = -1;
    public float PressInterval;

    public bool IsFingerDown => isFIngerDown;
    public Vector2 FingerScreenDownPos;
    public Vector2 FingerScreenUpPos;
    public Vector2 FingerScreenCurrenPos;
    public Vector2 FingerScreenOffset;
    public bool isReset = true;

    private ITwistHold twistHold;
    private bool isFIngerDown;

    private float downTime;
    private float PrePressTime;

    #region 订阅函数还没弄 获取手指不同状态下交互的UI

    #endregion

    private void Start()
    {
#if  UNITY_STANDALONE || UNITY_EDITOR
        twistHold = new MouseTwist(this);
#else
        twistHold = new FingerTwist(this);
#endif
    }
    public void ResetFingrInfo()
    {
        if (!isReset)
        {
            FingerScreenDownPos = Vector2.zero;
            FingerScreenCurrenPos = Vector2.zero;
            FingerScreenOffset = Vector2.zero;
            isReset = true;
        }
    }

    #region Mobile
    public void UpdateFingerInfo(Touch touch)
    {
        isReset = false;
        isFIngerDown = true;
        switch (touch.phase)
        {
            case TouchPhase.Began:
                OnFingerDown(touch);
                break;
            case TouchPhase.Moved:
                OnFingerMove(touch);
                break;
            case TouchPhase.Stationary:
                OnFingerStationary(touch);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnFingerUp(touch);
                break;
            default:
                break;
        }
    }
    private void OnFingerMove(Touch touch)
    {
        Vector2 scrrenOffset = touch.position - FingerScreenCurrenPos;
        FingerScreenOffset = scrrenOffset;
        FingerScreenCurrenPos = touch.position;
    }
    private void OnFingerDown(Touch touch)
    {
        FingerScreenDownPos = touch.position;
        FingerScreenCurrenPos = touch.position;
        FingerScreenOffset = Vector2.zero;
        isFIngerDown = true;
        downTime = Time.time;
    }
    private void OnFingerUp(Touch touch)
    {
        FingerScreenUpPos = touch.position;
        isFIngerDown = false;
        if ((Time.time - downTime) < PressInterval)
        {
            OnFingerPress(touch);
        }
    }
    private void OnFingerStationary(Touch touch)
    {
        FingerScreenCurrenPos = touch.position;
        FingerScreenOffset = Vector2.zero;
        if ((Time.time - downTime) >= PressInterval)
        {
            OnFingerHold(touch);
        }
    }
    private void OnFingerPress(Touch touch)
    {
        if ((Time.time - PrePressTime) < PressInterval)
        {
            OnFingerDoublePress(touch);
        }
        else
        {
            PrePressTime = Time.time;
        }
    }
    private void OnFingerDoublePress(Touch touch)
    {
        PrePressTime = 0;
    }
    private void OnFingerHold(Touch touch)
    {
    }
    #endregion

    #region PC
    public void UpdateMouseInfo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseLeftDown();
        }
        else if (Input.GetMouseButton(0))
        {
            OnMouserLeftMove();
            if ((Time.time - downTime) >= PressInterval)
            {
                OnMouseHold();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMouseLeftUp();
        }
    }
    private void OnMouseLeftDown()
    {
        FingerScreenDownPos = Input.mousePosition;
        FingerScreenCurrenPos = Input.mousePosition;
        FingerScreenOffset = Vector2.zero;
        isFIngerDown = true;
        downTime = Time.time;
    }
    private void OnMouseLeftUp()
    {
        FingerScreenUpPos = Input.mousePosition;
        isFIngerDown = false;
        if ((Time.time - downTime) < PressInterval)
        {
            OnMouseLeftPress();
        }
    }
    private void OnMouserLeftMove()
    {
        Vector2 scrrenOffset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - FingerScreenCurrenPos;
        FingerScreenOffset = scrrenOffset;
        FingerScreenCurrenPos = Input.mousePosition;
    }
    private void OnMouseLeftPress()
    {
        if ((Time.time - PrePressTime) < PressInterval)
        {
            OnMouseDoublePress();
        }
        else
        {
            PrePressTime = Time.time;
        }
    }
    private void OnMouseDoublePress()
    {
        PrePressTime = 0;
    }
    private void OnMouseHold()
    {
    }
    #endregion

}

/// <summary>
/// 限制只能单指控制
/// </summary>
class FingerTwist : ITwistHold
{
    private bool isHold = true;
    public bool IsHold { get => isHold; set => isHold = value; }
    private Action onUpdate;
    public Action OnUpdate { get => onUpdate; set => onUpdate += value; }
    private Action onStart;
    public Action OnStart { get => onStart; set => onStart += value; }
    private Action onCompleted;
    public Action OnCompleted { get => onCompleted; set => onCompleted += value; }

    private SingleFingerEventHandle fingerEventHandle;
    public FingerTwist(SingleFingerEventHandle fingerEventHandle)
    {
        onUpdate += UpdateFingerState;
        onStart = null;
        onCompleted = null;
        HangHook(PlayerLoopTiming.LastEarlyUpdate);
        this.fingerEventHandle = fingerEventHandle;
    }
    public bool MoveNext()
    {
        OnUpdate?.Invoke();
        if (!IsHold)
            onCompleted?.Invoke();
        return IsHold;
    }
    public void HangHook(PlayerLoopTiming playerLoopTiming)
    {
        PlayerLoopHelper.AddTwist(playerLoopTiming, this);
    }
    private void UpdateFingerState()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (SingleFingerEventHandle.Current.IsLimitingId < 0)
            {
                fingerEventHandle.UpdateFingerInfo(touch);
            }
            else if (SingleFingerEventHandle.Current.IsLimitingId == touch.fingerId)
            {
                fingerEventHandle.UpdateFingerInfo(touch);
            }
        }
        else
        {
            fingerEventHandle.ResetFingrInfo();
        }
    }
}

class MouseTwist : ITwistHold
{
    private bool isHold = true;
    public bool IsHold { get => isHold; set => isHold = value; }
    private Action onUpdate;
    public Action OnUpdate { get => onUpdate; set => onUpdate += value; }
    private Action onStart;
    public Action OnStart { get => onStart; set => onStart += value; }
    private Action onCompleted;
    public Action OnCompleted { get => onCompleted; set => onCompleted += value; }
    private SingleFingerEventHandle fingerEventHandle;
    public MouseTwist(SingleFingerEventHandle fingerEventHandle)
    {
        this.fingerEventHandle = fingerEventHandle;
        onUpdate += UpdateMouseState;
        onStart = null;
        onCompleted = null;
        HangHook(PlayerLoopTiming.LastEarlyUpdate);
    }
    public bool MoveNext()
    {
        OnUpdate?.Invoke();
        if (!IsHold)
            onCompleted?.Invoke();
        return IsHold;
    }
    public void HangHook(PlayerLoopTiming playerLoopTiming)
    {
        PlayerLoopHelper.AddTwist(playerLoopTiming, this);
    }
    private void UpdateMouseState()
    {
        fingerEventHandle.UpdateMouseInfo();
    }
}
