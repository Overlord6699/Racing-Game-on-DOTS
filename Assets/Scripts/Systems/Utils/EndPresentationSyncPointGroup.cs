using Unity.Entities;

namespace Drift
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
    public class EndPresentationSyncPointGroup : ComponentSystemGroup
    {
        
    }
}