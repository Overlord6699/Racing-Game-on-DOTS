using System.Collections.Generic;
using Drift.Procedural;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Drift
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(WheelSkidmarksSystem))]
    public class WheelSkidmarksMeshSystem : SystemBase
    {
        internal List<MeshUpdateJob> UpdateJobs = new List<MeshUpdateJob>();
        
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in Skidmarks skidmarks,
                in DynamicBuffer<SkidmarksPoint> points,
                in DynamicBuffer<SkidmarksSequence> sequences) =>
            {
                if (points.Length < 2) return;

                var meshUpdateJob = new MeshUpdateJob
                {
                    Entity = entity,
                    Lifetime = skidmarks.Lifetime,
                    Points = points.ToNativeArray(Allocator.TempJob),
                    Sequences = sequences.ToNativeArray(Allocator.TempJob),
                    Bounds = new NativeArray<Bounds>(1, Allocator.TempJob),
                    MeshDataArray = Mesh.AllocateWritableMeshData(1)
                };
                meshUpdateJob.JobHandle = meshUpdateJob.Schedule();
                UpdateJobs.Add(meshUpdateJob);
                
            }).WithoutBurst().Run();
        }

        internal struct MeshUpdateJob : IJob
        {
            public Entity Entity;
            
            public float Lifetime;
            
            public Mesh.MeshDataArray MeshDataArray;
            
            public JobHandle JobHandle;
            
            [DeallocateOnJobCompletion] [ReadOnly] [NoAlias]public NativeArray<SkidmarksPoint> Points;

            [DeallocateOnJobCompletion] [ReadOnly] [NoAlias]public NativeArray<SkidmarksSequence> Sequences;

            [WriteOnly] [NoAlias]public NativeArray<Bounds> Bounds;
            
            public void Execute()
            {
                using var builder = new NativeMeshBuilder<SkidmarkVertex>(Allocator.Temp);

                var currentSequenceStart = 0;
                for (int sequenceIndex = 0; sequenceIndex <= Sequences.Length; sequenceIndex++)
                {
                    var sequence = sequenceIndex == Sequences.Length
                        ? new SkidmarksSequence {End = Points.Length - 1}
                        : Sequences[sequenceIndex];
                    
                    var pointsInSequence = sequence.End - currentSequenceStart + 1;
                    if (pointsInSequence < 2)
                    {
                        currentSequenceStart = sequence.End + 1;
                        continue;
                    }
                    
                    var uvY = 0.0f;
                    var previousPosition = float3.zero;
                    for (var index = currentSequenceStart; index <= sequence.End; index++)
                    {
                        var point = Points[index];
                        var normal = point.Normal;
                        var isStartIndex = index == currentSequenceStart;
                        builder.AddVertex(new SkidmarkVertex
                        {
                            Position = point.Position + point.Right * (point.Width * 0.5f),
                            Normal = normal,
                            Uv = new float2(1, uvY),
                            Parameters = new float3(point.Intensity, point.CreationTime, Lifetime)
                        });
                        builder.AddVertex(new SkidmarkVertex
                        {
                            Position = point.Position - point.Right * (point.Width * 0.5f),
                            Normal = normal,
                            Uv = new float2(0, uvY),
                            Parameters = new float3(point.Intensity, point.CreationTime, Lifetime)
                        });
                        if (!isStartIndex)
                        {
                            uvY += math.distance(point.Position, previousPosition);
                            builder.AddQuadIndices(-2, -1, 1, 0);
                        }
                        builder.EndPart();

                        previousPosition = point.Position;
                    }
                    
                    currentSequenceStart = sequence.End + 1;
                }
                
                // Apply mesh
                var mesh = MeshDataArray[0];
                builder.ToMeshData(ref mesh);
                Bounds[0] = CalculateBounds(builder);
            }
            
            private static Bounds CalculateBounds(NativeMeshBuilder<SkidmarkVertex> builder)
            {
                if (builder.Vertices.Length == 0) return new Bounds();
                var result = new Bounds(builder.Vertices[0].Position, Vector3.zero);
                for (int i = 0; i < builder.Vertices.Length; i++)
                {
                    result.Encapsulate(builder.Vertices[i].Position);
                }

                return result;
            }
        }
        
        private struct SkidmarkVertex
        {
            [VertexAttribute(VertexAttribute.Position, 3)]
            public float3 Position;
            [VertexAttribute(VertexAttribute.Normal, 3)]
            public float3 Normal;
            [VertexAttribute(VertexAttribute.TexCoord0, 2)]
            public float2 Uv;
            [VertexAttribute(VertexAttribute.TexCoord1, 3)]
            public float3 Parameters;
        }
    }
}