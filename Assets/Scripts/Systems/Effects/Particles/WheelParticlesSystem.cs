using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Drift.Particles
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public class WheelParticlesSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((in WheelParticles wheelParticles, in WheelContact contact, 
                in WheelContactVelocity contactVelocity, in WheelOutput output, 
                in LocalToWorld transform) =>
            {
                var particles = EntityManager.GetComponentObject<ParticlesAuthoring>(wheelParticles.Active);
                
                if (!contact.IsInContact)
                {
                    Apply(particles, 0, 0);
                    return;
                }
                
                Apply(particles, math.length(contactVelocity.Value), output.Slip);
                
                SetComponent(wheelParticles.Active, new Translation {Value = transform.Position});
                SetComponent(wheelParticles.Active, new Rotation {Value = transform.Rotation});
                
            }).WithoutBurst().Run();
        }

        private static void Apply(ParticlesAuthoring particles, float speed, float slip)
        {
            var emissionRate = particles.SpeedToEmissionRemap.Remap(speed) * particles.SlipToEmissionRemap.Remap(slip);
            var emission = particles.Particles.emission;
            emission.enabled = emissionRate > 0.01f;
            if (emission.enabled)
            {
                emission.rateOverTimeMultiplier = emissionRate;
                var main = particles.Particles.main;
                main.startColor = particles.SlipToColor.Evaluate(slip);
            }
        }
    }
}