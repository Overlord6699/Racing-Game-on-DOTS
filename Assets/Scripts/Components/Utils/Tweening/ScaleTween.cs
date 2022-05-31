using Unity.Entities;
using Unity.Mathematics;

namespace Drift.Tweening
{
    public struct ScaleTween : IComponentData
    {
        public float3 EndValue;
        public float3 StartValue;
    }
}