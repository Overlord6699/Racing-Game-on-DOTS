using Unity.Entities;

namespace Drift.Particles
{
    public class WheelParticlesCheckSystem : SystemBase
    {
        private SwitchParticlesSystem switchSurfaceSoundSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            switchSurfaceSoundSystem = World.GetOrCreateSystem<SwitchParticlesSystem>();
        }
        
        protected override void OnUpdate()
        {
            var switchRequests = switchSurfaceSoundSystem.CreateBuffer();
            Dependency = Entities.WithAll<HasWheelParticles, Wheel>().ForEach((Entity entity, in Surface surface) =>
            {
                var switchSound = false;
                if (HasComponent<WheelParticles>(entity))
                {
                    var wheelSkidmarks = GetComponent<WheelParticles>(entity);
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
                    switchRequests.Enqueue(new SwitchParticlesSystem.Request
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