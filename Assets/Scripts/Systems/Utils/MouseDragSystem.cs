using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Drift
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class MouseDragSystem : SystemBase
    {
        private Controls controls;
        private Camera mainCamera;
        private BuildPhysicsWorld buildPhysicsWorld;
        private EntityCommandBufferSystem entityCommandBufferSystem;
        private bool isPickActive;
        private EntityQuery targetsQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            controls = ControlsTempAccess.Controls;
            buildPhysicsWorld = World.GetExistingSystem<BuildPhysicsWorld>();
            entityCommandBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ControlsTempAccess.Dispose();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            mainCamera = Camera.main;
            controls.Enable();
        }

        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            controls.Disable();
        }

        protected override void OnUpdate()
        {
            var mousePosition = controls.Debug.MousePosition.ReadValue<Vector2>();
            var cameraRaySource = mainCamera.ScreenPointToRay(mousePosition);
            var isPickInputEnabled = controls.Debug.MousePick.phase == InputActionPhase.Started;

            var isPickPerformed = isPickInputEnabled && !isPickActive;
            var isPickCompleted = !isPickInputEnabled && isPickActive;

            isPickActive = isPickInputEnabled;

            (float3 origin, float3 direction) cameraRay = ((float3) cameraRaySource.origin,
                (float3) cameraRaySource.direction);
        

            if (isPickPerformed)
            {
                var physicsWorld = buildPhysicsWorld.PhysicsWorld;
                var commands = entityCommandBufferSystem.CreateCommandBuffer();
                Dependency = Entities.WithReadOnly(physicsWorld).ForEach((in MouseDrag mouseDrag) =>
                {
                    if (!physicsWorld.CastRay(new RaycastInput
                    {
                        Start = cameraRay.origin,
                        End = cameraRay.origin + cameraRay.direction * mouseDrag.Distance,
                        Filter = CollisionFilter.Default
                    }, out var hit)) return;
                
                    // Есть пересечение
                    var translation = GetComponent<Translation>(hit.Entity).Value;
                    var rotation = GetComponent<Rotation>(hit.Entity).Value;

                    var transform = new RigidTransform(rotation, translation);
                    var invTransform = math.inverse(transform);
                    var localAnchor = math.transform(invTransform, hit.Position);
                
                    //var localAnchor = math.mul(math.inverse(rotation), hit.Position - translation);
                
                    commands.AddComponent(hit.Entity, new MouseDragTarget
                    {
                        Distance = hit.Fraction * mouseDrag.Distance,
                        Acceleration = mouseDrag.Acceleration,
                        LocalAnchor = localAnchor
                    });
                }).Schedule(Dependency);
                entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
            }
            else if (isPickCompleted && !targetsQuery.IsEmpty)
            {
                var commands = entityCommandBufferSystem.CreateCommandBuffer();
                Dependency = Entities.WithStoreEntityQueryInField(ref targetsQuery)
                    .WithAll<MouseDragTarget>().ForEach((Entity entity) =>
                    {
                        commands.RemoveComponent<MouseDragTarget>(entity);
                    }).Schedule(Dependency);
                entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
            }
        
            Entities.ForEach((ref Translation translation, in Rotation rotation) => {
            
            }).Schedule();
        }

        [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
        [UpdateBefore(typeof(BuildPhysicsWorld))]
        public class MouseDragSpringSystem : SystemBase
        {
            private Camera mainCamera;
            private IInputService input;

            [Inject]
            private void Inject(IInputService inputService)
            {
                input = inputService;
            }

            protected override void OnStartRunning()
            {
                base.OnStartRunning();
                mainCamera = Camera.main;
            }

            protected override void OnUpdate()
            {
                var mousePosition = input.Debug.MousePosition.ReadValue<Vector2>();
                var cameraRaySource = mainCamera.ScreenPointToRay(mousePosition);

                (float3 origin, float3 direction) cameraRay = ((float3) cameraRaySource.origin,
                    (float3) cameraRaySource.direction);

                var deltaTime = Time.DeltaTime;
            
                Entities.ForEach((ref PhysicsVelocity velocity, in PhysicsMass mass, in Translation translation, in Rotation rotation, in MouseDragTarget target) =>
                {
                    var worldBodyAnchor = translation.Value + math.rotate(rotation.Value, target.LocalAnchor);
                    var worldDragAnchor = cameraRay.origin + cameraRay.direction * target.Distance;

                    var worldVelocity = velocity.GetLinearVelocity(mass, translation, rotation, worldBodyAnchor);
                    
                    //
                    const float elasticity = 0.1f;
                    const float damping = 0.5f;
                    
                    var pointDiff = worldBodyAnchor - worldDragAnchor;
                    var deltaVelocity = -pointDiff * (elasticity / deltaTime) - damping * worldVelocity;

                    var effectiveMass = mass.GetEffectiveMass(translation, rotation, deltaVelocity, worldBodyAnchor);
                    var impulse = deltaVelocity * effectiveMass / math.rcp(mass.InverseMass);
                    
                    
                    const float maxAcceleration = 250.0f;
                    float maxImpulse = deltaTime * maxAcceleration;
                    ClampLength(ref impulse, maxImpulse);

                    velocity.Linear *= 0.95f;
                    velocity.Angular *= 0.95f;
                    velocity.ApplyAcceleration(mass, translation, rotation, impulse, worldBodyAnchor);
                }).Schedule();
            }

            private static void ClampLength(ref float3 vector, float maxLength)
            {
                var lengthsq = math.lengthsq(vector);
                if (lengthsq <= maxLength * maxLength)
                    return;
                var length = math.sqrt(lengthsq);
                vector.x /= length;
                vector.y /= length;
                vector.z /= length;
            }
        }
    }
}
