using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;
using Collider = Unity.Physics.Collider;

namespace Drift
{
    public class WheelAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float Mass = 20;
        [Header("Collider")]
        public float Radius;
        public float Width;
        public PhysicsCategoryTags BelongsTo;
        public PhysicsCategoryTags CollidesWith;
        
        [Header("Suspension")]
        public float SuspensionLength;
        public float Stiffness;
        public float Damping;

        [Header("Friction")]
        public AnimationCurve Longitudinal;
        public AnimationCurve Lateral;
        
        [Header("Brakes")]
        public float BrakeTorque;
        public float HandbrakeTorque;
        
        [Header("Controls")]
        public float MaxSteeringAngle;

        public float DriveRate;
        public float BrakeRate;
        public float HandbrakeRate;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(new ComponentType[] {
                typeof(Wheel),
                typeof(WheelOrigin),
                typeof(WheelInput),
                typeof(WheelContact),
                typeof(WheelSuspension),
                typeof(WheelContactVelocity),
                typeof(WheelOutput),
                typeof(WheelControllable),
                typeof(WheelFriction),
                typeof(WheelBrakes)
            }));
            
            var wheelCollider = CylinderCollider.Create(new CylinderGeometry
            {
                Center = float3.zero,
                Height = Width,
                Radius = Radius,
                BevelRadius = 0.1f,
                SideCount = 12,
                Orientation = quaternion.AxisAngle(math.up(), math.PI * 0.5f)
            }, new CollisionFilter
            {
                BelongsTo = BelongsTo.Value,
                CollidesWith = CollidesWith.Value
            });

            conversionSystem.BlobAssetStore.AddUniqueBlobAsset(ref wheelCollider);
            
            dstManager.SetComponentData(entity, new Wheel
            {
                Radius = Radius,
                Width = Width,
                SuspensionLength = SuspensionLength,
                Inertia = Mass * Radius * Radius * 0.5f,
                Collider = wheelCollider
            });
            dstManager.SetComponentData(entity, new WheelOrigin
            {
                Value = new RigidTransform(transform.localRotation, transform.localPosition)
            });
            dstManager.SetComponentData(entity, new WheelSuspension
            {
                Damping = Damping,
                Stiffness = Stiffness
            });

            var longitudinal = AnimationCurveBlob.Build(Longitudinal, 128, Allocator.Persistent);
            var lateral = AnimationCurveBlob.Build(Lateral, 128, Allocator.Persistent);

            conversionSystem.BlobAssetStore.AddUniqueBlobAsset(ref longitudinal);
            conversionSystem.BlobAssetStore.AddUniqueBlobAsset(ref lateral);
            
            dstManager.SetComponentData(entity, new WheelFriction
            {
                Longitudinal = longitudinal,
                Lateral = lateral
            });
            dstManager.SetComponentData(entity, new WheelControllable
            {
                MaxSteeringAngle = math.radians(MaxSteeringAngle),
                DriveRate = DriveRate,
                BrakeRate = BrakeRate,
                HandbrakeRate = HandbrakeRate
            });
            dstManager.SetComponentData(entity, new WheelBrakes
            {
                BrakeTorque = BrakeTorque,
                HandbrakeTorque = HandbrakeTorque
            });
        }
    }

    public struct Wheel : IComponentData
    {
        public float Radius;
        public float Width;
        public float SuspensionLength;
        public float Inertia;
        public BlobAssetReference<Collider> Collider;
    }

    public struct WheelOrigin : IComponentData
    {
        public RigidTransform Value;
    }

    public struct WheelInput : IComponentData
    {
        public float3 Up;
        public RigidTransform LocalTransform;
        public RigidTransform WorldTransform;
        public float MassMultiplier;
        public float Torque;
        public float Brake;
        public float Handbrake;
    }

    public struct WheelContact : IComponentData
    {
        public bool IsInContact;
        public float3 Point;
        public float3 Normal;
        public float Distance;
        public Entity Entity;
    }

    public struct WheelSuspension : IComponentData
    {
        public float Stiffness;
        public float Damping;
    }

    public struct WheelContactVelocity : IComponentData
    {
        public float3 Value;
    }

    public struct WheelOutput : IComponentData
    {
        public float3 SuspensionImpulse;
        public float3 FrictionImpulse;
        public float Rotation;
        public float RotationSpeed;
        public float Slip;
    }

    public struct WheelFriction : IComponentData
    {
        public BlobAssetReference<AnimationCurveBlob> Longitudinal;
        public BlobAssetReference<AnimationCurveBlob> Lateral;
    }

    public struct WheelControllable : IComponentData
    {
        public float MaxSteeringAngle;
        public float DriveRate;
        public float BrakeRate;
        public float HandbrakeRate;
    }

    public struct WheelBrakes : IComponentData
    {
        public float BrakeTorque;
        public float HandbrakeTorque;
    }
}
