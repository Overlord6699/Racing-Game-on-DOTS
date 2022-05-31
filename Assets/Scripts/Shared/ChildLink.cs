using Unity.Entities;

namespace Drift
{
    public struct ChildLink : IBufferElementData
    {
        public Entity Entity;
    }
}