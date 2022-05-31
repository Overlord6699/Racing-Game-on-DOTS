using Unity.Entities;

namespace Drift.Tasks
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TriggerSystem))]
    public class TouchTaskSystem : SystemBase
    {
        private EventBuilder<CompleteTaskRequest> completeTaskEvent;
        private EntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            completeTaskEvent = new EventBuilder<CompleteTaskRequest>(EntityManager);
            entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var completeTaskRequest = this.completeTaskEvent;
            var commands = entityCommandBufferSystem.CreateCommandBuffer();
            Entities.WithAll<Triggered, TouchTask>().WithNone<Completed>().ForEach((Entity entity) =>
            {
                completeTaskRequest.Raise(commands, new CompleteTaskRequest(entity));
            }).Run();
        }
    }
}