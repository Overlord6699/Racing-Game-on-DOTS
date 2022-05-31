using Unity.Entities;

namespace Drift
{
    [UpdateInGroup(typeof(PreEventsSystemGroup))]
    public class RemoveStateChangedSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<StateChanged>()
                .WithEntityQueryOptions(EntityQueryOptions.IncludeDisabled)
                .ForEach((in Entity entity) =>
                {
                    EntityManager.RemoveComponent<StateChanged>(entity);
                }).WithStructuralChanges().Run();
        }
    }
}