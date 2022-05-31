using System;
using Unity.Entities;
using UnityEngine;

namespace Drift
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlesAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private static int sortingIndex = 0;
        
        [NonSerialized]
        public ParticleSystem Particles;
        
        [SerializeField]
        public Remapping SlipToEmissionRemap = new Remapping(0.25f,1,0,25);
        [SerializeField]
        public Remapping SpeedToEmissionRemap = new Remapping(0,1,1,1);
        [SerializeField]
        public Gradient SlipToColor = new Gradient();

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            conversionSystem.AddHybridComponent(GetComponent<ParticleSystem>());
            conversionSystem.AddHybridComponent(this);
            dstManager.AddComponentObject(entity, this);
        }
        
        private void Awake()
        {
            Particles = GetComponent<ParticleSystem>();
            
            var renderer = GetComponent<ParticleSystemRenderer>();
            sortingIndex = (sortingIndex + 2) % 10;
            renderer.sortingFudge = sortingIndex;
        }
    }
}