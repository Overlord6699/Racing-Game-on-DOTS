using Unity.Entities;
using UnityEngine;

namespace Drift.Sounds
{
    public class SoundAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public SoundDefinition SoundDefinition;
        public float Volume = 1;
        public bool PlayOneShot;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.MakeSound(entity, SoundDefinition, Volume);
            if (PlayOneShot)
                dstManager.AddComponent<OneShot>(entity);
        }
    }
    
    public struct Sound : IComponentData
    {
        public int Id;
    }
    
    public struct Volume : IComponentData
    {
        public float Value;
    }
    
    public struct FloatParameter : IBufferElementData
    {
        public float Value;
    }
    
    public struct TrackPosition : IComponentData
    {
        public Entity Target;
    }
    
    public struct OneShot : IComponentData
    {
    }
}