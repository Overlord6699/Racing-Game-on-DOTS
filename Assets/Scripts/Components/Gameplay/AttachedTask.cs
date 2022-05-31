using Unity.Entities;

namespace Drift
{
    public struct AttachedTask : IComponentData
    {
        public Entity Entity;
    }
}