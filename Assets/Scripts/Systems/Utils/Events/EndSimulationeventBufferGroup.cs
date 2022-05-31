using Unity.Entities;

namespace Drift.Events
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    public class EndSimulationEventBufferGroup : ComponentSystemGroup
    {
        
    }
}