using Drift.Contacts;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Zenject;

namespace Drift.Sounds
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ContactSoundSystem : SystemBase
    {
        private ISurfaceService surfaceService;
        private EntityCommandBufferSystem entityCommandBufferSystem;

        [Inject]
        private void Inject(ISurfaceService surfaceService)
        {
            this.surfaceService = surfaceService;
        }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            entityCommandBufferSystem = 
                World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commands = entityCommandBufferSystem.CreateCommandBuffer();
            Entities.WithAll<Enter>().ForEach((in Contact contact) =>
            {
                
                if (HasComponent<Surface>(contact.EntityPair.A))
                    HandleSurfaceEntity(GetComponent<Surface>(contact.EntityPair.A), contact, commands);
                if (HasComponent<Surface>(contact.EntityPair.B))
                    HandleSurfaceEntity(GetComponent<Surface>(contact.EntityPair.B), contact, commands);
                
            }).WithoutBurst().Run();
        }
        
        private void HandleSurfaceEntity(in Surface surface, in Contact contact, EntityCommandBuffer commands)
        {
            var surfaceDefinition = surfaceService.Surfaces[surface.SurfaceIndex];
            var remappedVolume = math.clamp(math.unlerp(surfaceDefinition.ImpulseToVolumeRemap.x,
                surfaceDefinition.ImpulseToVolumeRemap.y, contact.CollisionData.Impulse), 0, 1);
            var prefabEntity = surfaceService.GetCollisionSoundPrefab(surface.SurfaceIndex);
                    
            if (prefabEntity != Entity.Null && remappedVolume > 0.01f)
            {
                SpawnSound(commands, prefabEntity, remappedVolume, 
                    contact.CollisionData.AverageContactPoint);
            }
        }
        
        private static void SpawnSound(EntityCommandBuffer commands, 
            Entity prefabEntity, float volume, float3 position)
        {
            commands.Instantiate(prefabEntity);
            commands.SetComponent(prefabEntity, new Volume
            {
                Value = volume
            });
            commands.SetComponent(prefabEntity, new Translation
            {
                Value = position
            });
            commands.AddComponent<OneShot>(prefabEntity);
        }
    }
}