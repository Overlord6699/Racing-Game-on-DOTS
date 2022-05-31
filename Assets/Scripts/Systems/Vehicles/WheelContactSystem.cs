using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Drift.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(VehicleToWheelsSystem))]
    [UpdateAfter(typeof(StepPhysicsWorld))]
    public class WheelContactSystem : SystemBase
    {
        private StepPhysicsWorld stepPhysicsWorld;
        private BuildPhysicsWorld buildPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();
            stepPhysicsWorld = World.GetExistingSystem<StepPhysicsWorld>();
            buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
        }

        protected override unsafe void OnUpdate()
        {
            var dep = JobHandle.CombineDependencies(Dependency, stepPhysicsWorld.GetOutputDependency());

            var physicsWorld = buildPhysicsWorld.PhysicsWorld;
            Dependency = Entities.WithReadOnly(physicsWorld).ForEach((ref WheelContact contact, in Wheel wheel, in WheelInput input) => { 
            
                var colliderCastInput = new ColliderCastInput
                {
                    Collider = (Collider*)wheel.Collider.GetUnsafePtr(),
                    Start = input.WorldTransform.pos,
                    End = input.WorldTransform.pos - input.Up * wheel.SuspensionLength,
                    Orientation = input.WorldTransform.rot
                };

                if (!physicsWorld.CastCollider(colliderCastInput, out var hit))
                {
                    contact.IsInContact = false;
                    contact.Distance = wheel.SuspensionLength;
                    return;
                }

                contact.IsInContact = true;
                contact.Point = hit.Position;
                contact.Normal = hit.SurfaceNormal;
                contact.Distance = hit.Fraction * wheel.SuspensionLength;
                contact.Entity = hit.Entity;

            }).Schedule(dep);
            
            buildPhysicsWorld.AddInputDependencyToComplete(Dependency);
        }
    }
}