using System;
namespace Tick{
public interface ITwistHold : ITwist
{
    bool IsHold { get; set; }
    Action OnUpdate { get; set; }
    Action OnStart { get; set; }
    Action OnCompleted { get; set; }
}
}