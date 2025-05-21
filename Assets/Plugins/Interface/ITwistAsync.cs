using System.Runtime.CompilerServices;
public interface ITwistAsync : ITwist, INotifyCompletion
{
    float InsertTime { get; }
    bool IsCompleted { get; }
    ITwistAsync GetAwaiter();
    void GetResult();
}
