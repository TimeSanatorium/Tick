using System;
public class HoldTwist : ITwistHold
{
    private bool isHold = true;
    public bool IsHold { get => isHold; set => isHold = value; }
    private Action onUpdate;
    public Action OnUpdate { get => onUpdate; set => onUpdate += value; }
    private Action onStart;
    public Action OnStart { get => onStart; set => onStart += value; }
    private Action onCompleted;
    public Action OnCompleted { get => onCompleted; set => onCompleted = value; }
    public HoldTwist(PlayerLoopTiming playerLoopTiming,Action onStart = null,Action onCompleted = null)
    {
        this.onStart += onStart;
        this.onCompleted += onCompleted;
        PlayerLoopHelper.AddTwist(playerLoopTiming, this);
    }
    public bool MoveNext()
    {
        onUpdate?.Invoke();
        return isHold;
    }
}
