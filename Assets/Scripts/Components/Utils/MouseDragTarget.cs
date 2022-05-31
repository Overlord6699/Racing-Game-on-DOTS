using Unity.Entities;
using Unity.Mathematics;

namespace Drift
{
    public struct MouseDragTarget : IComponentData
    {
        public float Distance;
        public float3 LocalAnchor;
        public float Acceleration;
    }
}