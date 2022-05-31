using Unity.Entities;

namespace Drift.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(WheelContactSystem))]
    public class WheelOnSurfaceSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<Surface>()
                .ForEach((Entity entity, in WheelContact contact) =>
                {
                    if (!contact.IsInContact)
                    {
                        SetComponent(entity, new Surface
                        {
                            SurfaceIndex = -1
                        });
                        return;
                    }
                    var surfaceIndex = 0;
                    if (HasComponent<Surface>(contact.Entity))
                        surfaceIndex = GetComponent<Surface>(contact.Entity).SurfaceIndex;
                    SetComponent(entity, new Surface
                    {
                        SurfaceIndex = surfaceIndex
                    });
                }).Schedule();
        }
    }
}