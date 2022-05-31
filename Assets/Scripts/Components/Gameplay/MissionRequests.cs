using Unity.Entities;

namespace Drift
{
    public struct ActiveMissionRequest : IComponentData
    {
        public Entity Entity;
        public Entity Player;
        public Entity Vehicle;

        public ActiveMissionRequest(Entity entity, Entity player, Entity vehicle)
        {
            Entity = entity;
            Player = player;
            Vehicle = vehicle;
        }
    }
    
    public struct CompleteMissionRequest : IComponentData
    {
        public Entity Entity;

        public CompleteMissionRequest(Entity entity)
        {
            Entity = entity;
        }
    }
    
    public struct CancelMissionRequest : IComponentData
    {
        public Entity Entity;

        public CancelMissionRequest(Entity entity)
        {
            Entity = entity;
        }
    }
}