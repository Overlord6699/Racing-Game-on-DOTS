using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace Drift
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class WheelsSkidmarksMeshApplySystem : SystemBase
    {
        private WheelSkidmarksMeshSystem skidmarksMeshSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            skidmarksMeshSystem = World.GetOrCreateSystem<WheelSkidmarksMeshSystem>();
        }
        
        protected override void OnUpdate()
        {
            for (var index = 0; index < skidmarksMeshSystem.UpdateJobs.Count; index++)
            {
                var meshUpdateJob = skidmarksMeshSystem.UpdateJobs[index];
                meshUpdateJob.JobHandle.Complete();
                
                var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(meshUpdateJob.Entity);
                Mesh.ApplyAndDisposeWritableMeshData(meshUpdateJob.MeshDataArray, renderMesh.mesh, 
                    MeshUpdateFlags.DontRecalculateBounds);
                
                renderMesh.mesh.bounds = meshUpdateJob.Bounds[0];
                meshUpdateJob.Bounds.Dispose();
                var bounds = renderMesh.mesh.bounds;
                EntityManager.SetComponentData(meshUpdateJob.Entity, new RenderBounds
                {
                    Value = new AABB
                    {
                        Center = bounds.center,
                        Extents = bounds.extents
                    }
                });
            }
            skidmarksMeshSystem.UpdateJobs.Clear();
        }
    }
}