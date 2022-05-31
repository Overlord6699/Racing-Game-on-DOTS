using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class SurfaceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public SurfaceDefinition Surface;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Surface
            {
                SurfaceIndex = Surface.RuntimeIndex
            });
        }
    }
    
    public struct Surface : IComponentData
    {
        public int SurfaceIndex;
    }
}