using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Drift
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(BuildPhysicsWorld))]
    public class VehicleHelpersSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((ref PhysicsVelocity velocity, in Vehicle vehicle,
                in VehicleInput input, in VehicleHelpers helpers, in Rotation rotation) =>
            {
                
                var steeringAbs = math.abs(input.Steering);
                // Counter steering
                var rotationSpeed = velocity.Angular.y;
                if (rotationSpeed * input.Steering < 0)
                {
                    var counterSteeringInputRate = steeringAbs * math.clamp(input.Throttle, 0, 1)
                                                               * helpers.CounterSteeringRate;
                    velocity.Angular.y = math.lerp(velocity.Angular.y, 0, deltaTime * counterSteeringInputRate);
                }
                
                // Forward stabilization
                var forwardMovementRate =
                    math.abs(math.dot(math.forward(rotation.Value), math.normalizesafe(velocity.Linear)));
                var steeringRate = math.saturate((0.1f - steeringAbs) * 10f);
                var forwardStabilizationInputRate = steeringRate * forwardMovementRate
                                                                 * helpers.ForwardStabilizationRate;
                velocity.Angular.y = math.lerp(velocity.Angular.y, 0, deltaTime * forwardStabilizationInputRate);

            }).Schedule();
        }
    }
}