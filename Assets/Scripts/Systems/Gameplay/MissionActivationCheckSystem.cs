using Unity.Entities;
using Zenject;

namespace Drift
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class MissionActivationCheckSystem : SystemBase
    {
        private IInputService inputService;
        private EntityCommandBufferSystem entityCommandBufferSystem;
        private EventBuilder<ActiveMissionRequest> activateMissionEvent;
        
        [Inject]
        private void Inject(IInputService inputService)
        {
            this.inputService = inputService;
        }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            entityCommandBufferSystem = World
                .GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
            activateMissionEvent = new EventBuilder<ActiveMissionRequest>(EntityManager);
        }
        
        protected override void OnUpdate()
        {
            var isInteractionInitiated = inputService.Player.Interact.triggered;
            if (isInteractionInitiated)
            {
                var commands = entityCommandBufferSystem.CreateCommandBuffer();
                var activateMissionRequest = this.activateMissionEvent;
                
                Entities.ForEach((TriggerEvent triggerEvent) =>
                {
                    if (HasComponent<Mission>(triggerEvent.Trigger)
                        && HasComponent<AttachedPlayer>(triggerEvent.Source))
                    {
                        activateMissionRequest.Raise(commands, new ActiveMissionRequest(
                            triggerEvent.Trigger,
                            GetComponent<AttachedPlayer>(triggerEvent.Source).Entity,
                            triggerEvent.Source
                        ));
                    }
                }).Run();
            }
        }
    }
}