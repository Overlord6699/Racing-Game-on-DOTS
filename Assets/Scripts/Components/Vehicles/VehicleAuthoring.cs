using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Drift
{
    public class VehicleAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Engine")]
        public AnimationCurve Torque;
        public float TransmissionRate;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Vehicle),
                typeof(VehicleInput),
                typeof(VehicleEngine),
                typeof(VehicleOutput)
                ));
            var vehicle = new Vehicle();

            foreach (var wheelAuthoring in GetComponentsInChildren<WheelAuthoring>(true))
            {
                vehicle.Wheels.Add(conversionSystem.GetPrimaryEntity(wheelAuthoring));
            }
            
            dstManager.SetComponentData(entity, vehicle);

            var torque = AnimationCurveBlob.Build(Torque, 128, Allocator.Persistent);
            conversionSystem.BlobAssetStore.AddUniqueBlobAsset(ref torque);
            
            dstManager.SetComponentData(entity, new VehicleEngine
            {
                Torque = torque,
                TransmissionRate = TransmissionRate
            });
        }
    }

    public struct Vehicle : IComponentData
    {
        public FixedList64<Entity> Wheels;
    }

    public struct VehicleInput : IComponentData
    {
        public float Steering;
        public float Throttle;
        public float Brake;
        public float Handbrake;
        public float Load;
        public ThrottleMode ThrottleMode;
    }

    public enum ThrottleMode
    {
        AccelerationForward,
        AccelerationBackward,
        Braking
    }

    public struct VehicleEngine : IComponentData
    {
        public BlobAssetReference<AnimationCurveBlob> Torque;
        public float TransmissionRate;
    }

    public struct VehicleOutput : IComponentData
    {
        public float EngineRotationRate;
        public float MaxWheelRotationSpeed;
        public float AverageWheelRotationSpeed;
        public float3 LocalVelocity;
    }
}