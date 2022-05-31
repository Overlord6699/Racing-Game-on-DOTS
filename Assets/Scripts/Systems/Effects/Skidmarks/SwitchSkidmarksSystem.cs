using Drift.Events;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using Zenject;

namespace Drift.Systems
{
    [UpdateInGroup(typeof(EndSimulationEventBufferGroup))]
    public class SwitchSkidmarksSystem : EventBufferSystem<SwitchSkidmarksSystem.Request>
    {
        private ISurfaceService surfaceService;

        [Inject]
        private void Inject(ISurfaceService surfaceService)
        {
            this.surfaceService = surfaceService;
        }
        
        protected override void Handle(Request request)
        {
            SkidmarksPoint? previousPoint = null;
            if (EntityManager.HasComponent<WheelSkidmarks>(request.Wheel))
            {
                var wasEntity = EntityManager.GetComponentData<WheelSkidmarks>(request.Wheel).ActiveSkidmarks;
                previousPoint = DetachSkidmarksAndGetLastPoint(request.Wheel, wasEntity);
                EntityManager.RemoveComponent<WheelSkidmarks>(request.Wheel);
            }
            
            if (request.SurfaceIndex < 0) return;
            var prefab = surfaceService.GetSkidmarksPrefab(request.SurfaceIndex);
            
            if (prefab != Entity.Null)
            {
                var entity = EntityManager.Instantiate(prefab);
                var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
                renderMesh.mesh = new Mesh();
                EntityManager.SetSharedComponentData(entity, renderMesh);
                AttachSkidmarks(request.Wheel, entity, request.SurfaceIndex);
                
                if (previousPoint.HasValue)
                {
                    var points = GetBuffer<SkidmarksPoint>(entity);
                    points.Add(previousPoint.Value);
                }
            }
        }
        
        private SkidmarksPoint? DetachSkidmarksAndGetLastPoint(Entity wheel, Entity skidmarks)
        {
            EntityManager.RemoveComponent<WheelSkidmarks>(wheel);
            
            SkidmarksPoint? result = null;
            
            var skidmarksPoints = EntityManager.GetBuffer<SkidmarksPoint>(skidmarks);
            if (!skidmarksPoints.IsEmpty) result = skidmarksPoints[skidmarksPoints.Length - 1];
            
            EntityManager.RemoveComponent<SkidmarksPoint>(skidmarks);
            EntityManager.RemoveComponent<SkidmarksSequence>(skidmarks);
            
            EntityManager.AddComponentData(skidmarks, new Lifetime
            {
                Time = EntityManager.GetComponentData<Skidmarks>(skidmarks).Lifetime
            });

            return result;
        }
        
        private void AttachSkidmarks(Entity wheel, Entity skidmarks, int surfaceIndex)
        {
            var wheelSkidmarks = new WheelSkidmarks
            {
                SurfaceIndex = surfaceIndex,
                ActiveSkidmarks = skidmarks
            };
            EntityManager.AddComponentData(wheel, wheelSkidmarks);
        }
        
        public struct Request
        {
            public Entity Wheel;
            public int SurfaceIndex;
        }
    }
}