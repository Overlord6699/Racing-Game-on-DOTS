using Drift.Sounds;
using Unity.Entities;
using Unity.Mathematics;

namespace Drift.SurfaceSounds
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public class WheelSurfaceSoundSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<HasWheelSurfaceSound>().ForEach((ref WheelSurfaceSound surfaceSound, in WheelContact contact, 
                in WheelContactVelocity contactVelocity, in WheelOutput output) =>
            {
                var parametersBuffer = GetBuffer<FloatParameter>(surfaceSound.ActiveSound);
                if (parametersBuffer.Length < 2) return;
                
                if (!contact.IsInContact)
                {
                    parametersBuffer[0] = new FloatParameter {Value = 0};
                    parametersBuffer[1] = new FloatParameter {Value = 0};
                    return;
                }
                
                parametersBuffer[0] = new FloatParameter {Value = math.length(contactVelocity.Value)};
                parametersBuffer[1] = new FloatParameter {Value = output.Slip};
            }).Run();
        }
    }
}