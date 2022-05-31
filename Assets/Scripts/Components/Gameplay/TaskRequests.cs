using Unity.Entities;

namespace Drift
{
    public struct ActivateTaskRequest : IComponentData
    {
        public Entity Entity;
        public Entity Player;
        public Entity Vehicle;

        public ActivateTaskRequest(Entity entity, Entity player, Entity vehicle)
        {
            Entity = entity;
            Player = player;
            Vehicle = vehicle;
        }
    }
    
    public struct CompleteTaskRequest : IComponentData
    {
        public Entity Entity;

        public CompleteTaskRequest(Entity entity)
        {
            Entity = entity;
        }
    }
    
    public struct DisableTaskRequest : IComponentData
    {
        public Entity Entity;

        public DisableTaskRequest(Entity entity)
        {
            Entity = entity;
        }
    }
}