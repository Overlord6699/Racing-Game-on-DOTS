using Unity.Entities;

namespace Drift
{
    public struct ParentLink : IComponentData
    {
        public Entity Entity;
    }
}