using Drift.Events;
using Drift.Sounds;
using Unity.Entities;
using Zenject;

namespace Drift.SurfaceSounds
{
    [UpdateInGroup(typeof(EndSimulationEventBufferGroup))]
    public class SwitchSurfaceSoundSystem : EventBufferSystem<SwitchSurfaceSoundSystem.Request>
    {
        private ISurfaceService surfaceService;

        [Inject]
        private void Inject(ISurfaceService surfaceService)
        {
            this.surfaceService = surfaceService;
        }
        
        protected override void Handle(Request request)
        {
            if (EntityManager.HasComponent<WheelSurfaceSound>(request.Wheel))
            {
                var wasEntity = EntityManager.GetComponentData<WheelSurfaceSound>(request.Wheel).ActiveSound;
                DetachSound(request.Wheel, wasEntity);
            }
            
            if (request.SurfaceIndex < 0) return;
            
            var prefab = surfaceService.GetSurfaceSoundPrefab(request.SurfaceIndex);
            
            if (prefab != Entity.Null)
            {
                var entity = EntityManager.Instantiate(prefab);
                AttachSound(request.Wheel, entity, request.SurfaceIndex);
            }
        }
        
        private void DetachSound(Entity wheel, Entity sound)
        {
            EntityManager.DestroyEntity(sound);
            EntityManager.RemoveComponent<WheelSurfaceSound>(wheel);
        }
        
        private void AttachSound(Entity wheel, Entity entity, int requestSurfaceIndex)
        {
            EntityManager.AddComponentData(wheel, new WheelSurfaceSound
            {
                SurfaceIndex = requestSurfaceIndex,
                ActiveSound = entity
            });
            EntityManager.AddComponentData(entity, new TrackPosition
            {
                Target = wheel
            });
        }
        
        public struct Request
        {
            public Entity Wheel;
            public int SurfaceIndex;
        }
    }
}