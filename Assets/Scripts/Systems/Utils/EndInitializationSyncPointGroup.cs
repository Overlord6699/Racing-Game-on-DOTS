using Unity.Entities;

namespace Drift
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public class EndInitializationSyncPointGroup : ComponentSystemGroup
    {
        
    }
}