using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class VehicleHelpersAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float CounterSteeringRate = 5;
        public float ForwardStabilizationRate = 5;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity,  new VehicleHelpers
            {
                CounterSteeringRate = CounterSteeringRate,
                ForwardStabilizationRate = ForwardStabilizationRate
            });
        }
    }
    
    public struct VehicleHelpers : IComponentData
    {
        public float CounterSteeringRate;
        public float ForwardStabilizationRate;
    }
}