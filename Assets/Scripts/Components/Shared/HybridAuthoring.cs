using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class HybridAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Component[] Components;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            foreach (var component in Components)
            {
                conversionSystem.AddHybridComponent(component);
            }
        }
    }
}