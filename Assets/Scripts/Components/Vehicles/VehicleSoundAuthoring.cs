using Drift.Sounds;
using Unity.Assertions;
using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class VehicleSoundAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public SoundDefinition EngineSound;
        public float EngineVolume = 1;
        public float RotationChangeSpeed = 5;
        public float LoadChangeSpeed = 5;
        public float RotationMultiplier = 2;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Assert.IsTrue(EngineSound.FloatParameters != null && EngineSound.FloatParameters.Length == 2, 
                "Two engine sound parameters expected");
            
            var engineSoundEntity = conversionSystem.CreateAdditionalEntity(this);
            dstManager.MakeSound(engineSoundEntity, EngineSound, EngineVolume);
            dstManager.Link(entity, engineSoundEntity);
            
            dstManager.AddComponentData(engineSoundEntity, new TrackPosition
            {
                Target = entity
            });
            
            dstManager.AddComponentData(entity, new VehicleSound
            {
                Engine = engineSoundEntity,
                LoadChangeSpeed = LoadChangeSpeed,
                RotationChangeSpeed = RotationChangeSpeed,
                RotationMultiplier = RotationMultiplier
            });
        }
    }
    
    public struct VehicleSound : IComponentData
    {
        public Entity Engine;
        public float RotationChangeSpeed;
        public float LoadChangeSpeed;
        public float RotationMultiplier;
    }
}