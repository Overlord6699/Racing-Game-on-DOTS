using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class WheelParticlesAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Surface),
                typeof(HasWheelParticles)
            ));
        }
    }
        
    public struct HasWheelParticles : IComponentData {}
    
    public struct WheelParticles : IComponentData
    {
        public int SurfaceIndex;
        public Entity Active;
    }
}