using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public VehicleAuthoring Vehicle;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Player),
                typeof(AttachedVehicle)
            ));
            
            var vehicleEntity = conversionSystem.GetPrimaryEntity(Vehicle);
            dstManager.SetComponentData(entity, new AttachedVehicle
            {
                Entity = vehicleEntity
            });
            dstManager.AddComponentData(vehicleEntity, new AttachedPlayer
            {
                Entity = entity
            });
        }
    }
    
    public struct Player : IComponentData
    {
    }
    
    public struct AttachedVehicle : IComponentData
    {
        public Entity Entity;
    }

    public struct AttachedPlayer : IComponentData
    {
        public Entity Entity;
    }
}