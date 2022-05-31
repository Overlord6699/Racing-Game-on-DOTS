using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Drift
{
    [UpdateInGroup(typeof(EventsSystemGroup))]
    public class MissionActivateSystem : SystemBase
    {
        private EntityQuery requests;

        protected override void OnUpdate()
        {
            Entities
                .WithStoreEntityQueryInField(ref requests)
                .ForEach((in ActiveMissionRequest activeMissionRequest) =>
                {
                    ActivateMission(EntityManager, activeMissionRequest.Entity, 
                        activeMissionRequest.Player, activeMissionRequest.Vehicle);
                }).WithStructuralChanges().Run();
            EntityManager.DestroyEntity(requests);
        }

        public static void ActivateMission(EntityManager entityManager, 
            Entity missionEntity, Entity player, Entity vehicle)
        {
            Debug.Log("Mission activated: " + entityManager.GetName(missionEntity));
            
            entityManager.AddComponents(missionEntity, new ComponentTypes(
                ComponentType.ReadWrite<Active>(),
                ComponentType.ReadWrite<AttachedPlayer>(),
                ComponentType.ReadWrite<AttachedVehicle>()
            ));
            
            entityManager.SetComponentData(missionEntity, new AttachedPlayer { Entity = player });
            entityManager.SetComponentData(missionEntity, new AttachedVehicle { Entity = vehicle });
            
            var missionsInactiveMissionsQuery = entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<Mission>(),
                ComponentType.Exclude<Active>());
            using var otherMissions = missionsInactiveMissionsQuery.ToEntityArray(Allocator.Temp);
            foreach (var otherMission in otherMissions)
            {
                entityManager.SetEnabled(otherMission, false);
            }

            var mission = entityManager.GetComponentData<Mission>(missionEntity);
            TaskActivateSystem.ActivateTask(entityManager, 
                mission.RootTask, player, vehicle);
            
            if (mission.Scene != Entity.Null)
                entityManager.ActivateScene(mission.Scene);
        }

    }
}