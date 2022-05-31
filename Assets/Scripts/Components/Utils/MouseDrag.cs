using Unity.Entities;

namespace Drift
{
    [GenerateAuthoringComponent]
    public struct MouseDrag : IComponentData
    {
        public float Distance;
        public float Acceleration;
    }
}
