using System.Runtime.CompilerServices;
namespace Tick{
public interface ITwistAsync : ITwist, INotifyCompletion
{
    float InsertTime { get; }
    bool IsCompleted { get; }
    ITwistAsync GetAwaiter();
    void GetResult();
}

}