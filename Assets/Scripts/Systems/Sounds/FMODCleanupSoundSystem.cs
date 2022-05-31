using Unity.Entities;

namespace Drift.Sounds
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class FMODCleanupSoundSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            commandBufferSystem = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commands = commandBufferSystem.CreateCommandBuffer();
            Entities.WithNone<Sound>().ForEach((Entity entity, FMODSound sound) =>
            {
                
                sound.Event.release();
                commands.RemoveComponent<FMODSound>(entity);
                
            }).Run();
        }
    }
}