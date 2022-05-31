﻿using Unity.Mathematics;

namespace Drift
{
    public readonly struct DirectedPoint
    {
        public readonly float3 Position;
        public readonly float3 Direction;

        public DirectedPoint(float3 position, float3 directionNormalized)
        {
            Position = position;
            Direction = directionNormalized;
        }
    }
}