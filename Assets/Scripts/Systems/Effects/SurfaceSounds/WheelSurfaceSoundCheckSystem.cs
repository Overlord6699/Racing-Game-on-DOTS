using Unity.Entities;

namespace Drift.SurfaceSounds
{
    public class WheelSurfaceSoundCheckSystem : SystemBase
    {
        private SwitchSurfaceSoundSystem switchSurfaceSoundSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            switchSurfaceSoundSystem = World.GetOrCreateSystem<SwitchSurfaceSoundSystem>();
        }
        
        protected override void OnUpdate()
        {
            var switchRequests = switchSurfaceSoundSystem.CreateBuffer();
            Dependency = Entities.WithAll<HasWheelSurfaceSound>().ForEach((Entity entity, in Surface surface) =>
            {
                var switchSound = false;
                if (HasComponent<WheelSurfaceSound>(entity))
                {
                    var wheelSkidmarks = GetComponent<WheelSurfaceSound>(entity);
                    if (wheelSkidmarks.SurfaceIndex != surface.SurfaceIndex && surface.SurfaceIndex >= 0)
                    {
                        switchSound = true;
                    }
                }
                else
                {
                    switchSound = surface.SurfaceIndex >= 0;
                }
                
                if (switchSound)
                {
                    switchRequests.Enqueue(new SwitchSurfaceSoundSystem.Request
                    {
                        Wheel = entity,
                        SurfaceIndex = surface.SurfaceIndex
                    });
                }

            }).Schedule(Dependency);
            switchSurfaceSoundSystem.AddProducerJob(Dependency);
        }
    }
}