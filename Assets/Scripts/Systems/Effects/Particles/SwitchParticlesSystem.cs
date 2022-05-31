using System.Collections.Generic;
using Drift.Events;
using Unity.Entities;
using Zenject;

namespace Drift.Particles
{
    [UpdateInGroup(typeof(EndSimulationEventBufferGroup))]
    public class SwitchParticlesSystem : EventBufferSystem<SwitchParticlesSystem.Request>
    {
        private ISurfaceService surfaceService;
        private Stack<Entity>[] particlePoolsBySurfaceIndex;
        
        [Inject]
        private void Inject(ISurfaceService surfaceService)
        {
            this.surfaceService = surfaceService;
            particlePoolsBySurfaceIndex = new Stack<Entity>[surfaceService.Surfaces.Length];
        }
        
        protected override void Handle(Request request)
        {
            if (EntityManager.HasComponent<WheelParticles>(request.Wheel))
            {
                var wheelParticles = EntityManager.GetComponentData<WheelParticles>(request.Wheel);
                DetachParticles(request.Wheel, wheelParticles.Active, wheelParticles.SurfaceIndex);
            }
            
            if (request.SurfaceIndex < 0) return;
            
            var prefab = surfaceService.Surfaces[request.SurfaceIndex].Particles;
            
            if (prefab != Entity.Null)
            {
                AttachParticles(request.Wheel, prefab, request.SurfaceIndex);
            }
        }
        
        private void AttachParticles(Entity wheel, Entity prefab, int requestSurfaceIndex)
        {
            var entity = Take(requestSurfaceIndex, prefab);
            EntityManager.AddComponentData(wheel, new WheelParticles
            {
                SurfaceIndex = requestSurfaceIndex,
                Active = entity
            });
        }
        
        private void DetachParticles(Entity wheel, Entity particles, int surfaceIndex)
        {
            Release(surfaceIndex, particles);
            EntityManager.RemoveComponent<WheelParticles>(wheel);
        }
        
        private Entity Take(int surfaceIndex, Entity prefab)
        {
            var pool = particlePoolsBySurfaceIndex[surfaceIndex];
            Entity result;
            if (pool != null && pool.Count > 0)
            {
                result = pool.Pop();
                EntityManager.SetEnabled(result, true);
            }
            else
            {
                result = EntityManager.Instantiate(prefab);
            }
            
            return result;
        }
        
        private void Release(int surfaceIndex, Entity particles)
        {
            var pool = particlePoolsBySurfaceIndex[surfaceIndex];
            if (pool == null)
            {
                pool = new Stack<Entity>();
                particlePoolsBySurfaceIndex[surfaceIndex] = pool;
            }
            EntityManager.SetEnabled(particles, false);
            pool.Push(particles);
        }
        
        public struct Request
        {
            public Entity Wheel;
            public int SurfaceIndex;
        }
    }
}