using Unity.Collections;
using Unity.Mathematics;

namespace Drift.Contacts
{
    public struct CollisionData
    {
        public float Impulse;
        public float3 Normal;
        public float3 AverageContactPoint;
        public FixedList64<float3> ContactPoints;
    }
}