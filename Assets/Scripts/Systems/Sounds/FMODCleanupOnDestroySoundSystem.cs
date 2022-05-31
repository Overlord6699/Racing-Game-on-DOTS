using Unity.Entities;

namespace Drift.Sounds
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class FMODCleanupOnDestroySoundSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Entities.ForEach((Entity entity, FMODSound sound) =>
            {
                sound.Event.release();
            }).WithStructuralChanges().Run();
        }
        
        protected override void OnUpdate()
        {
        }
    }
}