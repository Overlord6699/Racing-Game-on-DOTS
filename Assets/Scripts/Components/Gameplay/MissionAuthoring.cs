﻿using Unity.Entities;
using UnityEngine;

namespace Drift
{
    public class MissionAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public TaskAuthoring Task;
        public MissionSceneAuthoring TaskObjects;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Mission)
            ));

            var taskEntity = conversionSystem.GetPrimaryEntity(Task);
            dstManager.AddComponentData(taskEntity, new ParentLink
            {
                Entity = entity
            });
            dstManager.SetComponentData(entity, new Mission
            {
                RootTask = taskEntity,
                Scene = TaskObjects != null ? conversionSystem.GetPrimaryEntity(TaskObjects) : Entity.Null
            });
        }
    }
    
    public struct Mission : IComponentData
    {
        public Entity RootTask;
        public Entity Scene;
    }
}