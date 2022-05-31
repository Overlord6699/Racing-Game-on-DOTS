using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Drift
{
    [UpdateInGroup(typeof(EventsSystemGroup))]
    public class TaskActivateSystem : SystemBase
    {
        private EntityQuery requests;
        
        protected override void OnUpdate()
        {
            Entities
                .WithStoreEntityQueryInField(ref requests)
                .ForEach((in ActivateTaskRequest activeTaskRequest) =>
                {
                    
                    ActivateTask(EntityManager, activeTaskRequest.Entity, 
                        activeTaskRequest.Player, activeTaskRequest.Vehicle);

                }).WithStructuralChanges().Run();
            EntityManager.DestroyEntity(requests);
        }

        public static void ActivateTask(EntityManager entityManager, 
            Entity taskEntity, Entity player, Entity vehicle)
        {
            Debug.Log("Task activated: " + entityManager.GetName(taskEntity));
            
            entityManager.SetEnabled(taskEntity, true);
            entityManager.RemoveComponent<Completed>(taskEntity);
            entityManager.AddComponents(taskEntity, new ComponentTypes(
                ComponentType.ReadWrite<StateChanged>(),
                ComponentType.ReadWrite<AttachedPlayer>(),
                ComponentType.ReadWrite<AttachedVehicle>()
            ));
            
            entityManager.SetComponentData(taskEntity, new AttachedPlayer { Entity = player });
            entityManager.SetComponentData(taskEntity, new AttachedVehicle { Entity = vehicle });

            if (entityManager.HasComponent<TaskGroup>(taskEntity))
            {
                var taskGroup = entityManager.GetComponentData<TaskGroup>(taskEntity);
                var children = entityManager.GetBuffer<ChildLink>(taskEntity);

                if (!children.IsEmpty)
                {
                    switch (taskGroup.CompletionStrategy)
                    {
                        case GroupCompletionStrategy.Sequence:
                        {
                            ActivateTask(entityManager, children[0].Entity, player, vehicle);
                            break;
                        }
                        case GroupCompletionStrategy.Parallel:
                        {
                            using var childrenCopy = children.ToNativeArray(Allocator.Temp);
                            foreach (var child in childrenCopy)
                            {
                                ActivateTask(entityManager, child.Entity, player, vehicle);
                            }
                            break;
                        }
                    }
                }
            }
            
            entityManager.UpdateIndicators(taskEntity);
        }
    }
}