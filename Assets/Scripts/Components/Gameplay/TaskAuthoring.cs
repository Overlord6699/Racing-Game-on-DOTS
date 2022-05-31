using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class TaskAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public virtual void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Task),
                typeof(Disabled)
            ));
        }
    }

    public struct Task : IComponentData
    {
    }
}