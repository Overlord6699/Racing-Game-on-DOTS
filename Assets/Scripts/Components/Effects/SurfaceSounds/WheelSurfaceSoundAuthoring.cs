using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class WheelSurfaceSoundAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Surface),
                typeof(HasWheelSurfaceSound)
            ));
        }
    }
    
    public struct HasWheelSurfaceSound : IComponentData { }
    
    public struct WheelSurfaceSound : IComponentData
    {
        public int SurfaceIndex;
        public Entity ActiveSound;
    }
}