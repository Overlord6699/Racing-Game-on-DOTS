using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Drift
{
    public class SkidmarksAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public int Capacity = 512;
        public float Lifetime;
        public float2 SlipToIntensityRemap = new float2(0,1);
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Skidmarks),
                typeof(SkidmarksPoint),
                typeof(SkidmarksSequence)
            ));
            var points = dstManager.GetBuffer<SkidmarksPoint>(entity);
            points.Capacity = Capacity;
            dstManager.SetComponentData(entity, new Skidmarks
            {
                Lifetime = Lifetime,
                Capacity = Capacity,
                SlipToIntensityRemap = SlipToIntensityRemap
            });
        }
    }
    
    public struct Skidmarks : IComponentData
    {
        public float Lifetime;
        public int Capacity;
        public float2 SlipToIntensityRemap;
    }
    
    public struct SkidmarksPoint : IBufferElementData
    {
        public float3 Position;
        public float3 Normal;
        public float3 Right;
        public float Width;
        public float Intensity;
        public float CreationTime;
    }
    
    public struct SkidmarksSequence : IBufferElementData
    {
        public int End;
    }
}