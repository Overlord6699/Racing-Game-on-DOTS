using Drift.Contacts;
using Unity.Entities;

namespace Drift
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class MissionIndicatorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithNone<Active>().WithAll<Enter, Trigger, Mission>()
                .ForEach((in MissionIndicator indicator) =>
                {
                    indicator.SetState(true);
                }).WithoutBurst().Run();
            Entities.WithAll<Exit, Trigger, Mission>()
                .ForEach((in MissionIndicator indicator) =>
                {
                    indicator.SetState(false);
                }).WithoutBurst().Run();
        }
    }
}