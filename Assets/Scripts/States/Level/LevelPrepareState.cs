using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Drift.States
{
    public class LevelPrepareState : IState
    {
        private readonly IStateMachine<LevelTrigger> states;
        private readonly World world;
        
        public LevelPrepareState(IStateMachine<LevelTrigger> states, World world)
        {
            this.states = states;
            this.world = world;
        }
        
        public void OnEnter()
        {
            var entityManager = world.EntityManager;
            var playerEntity = entityManager.CreateEntity(
                ComponentType.ReadWrite<Player>(),
                ComponentType.ReadWrite<AttachedVehicle>());
            
            world.Update();
            
            var vehicleQuery = entityManager.CreateEntityQuery(ComponentType.ReadOnly<Vehicle>());
            using var vehicles = vehicleQuery.ToEntityArray(Allocator.Temp);
            
            if (vehicles.Length == 0)
            {
                Debug.LogError("Vehicles not found");
                return;
            }
            var vehicleEntity = vehicles[0];
            entityManager.SetComponentData(playerEntity, new AttachedVehicle
            {
                Entity = vehicleEntity
            });
            entityManager.AddComponentData(vehicleEntity, new AttachedPlayer
            {
                Entity = playerEntity
            });
            
            states.Fire(LevelTrigger.Play);
        }

        public void OnExit()
        {
        }
    }
}