using FMODUnity;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Drift.Sounds
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class FMODTrackPositionSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((in FMODSound sound, in TrackPosition trackPosition) =>
            {
                sound.Event.set3DAttributes(
                    ((Vector3) GetComponent<LocalToWorld>(trackPosition.Target).Position).To3DAttributes());
            }).Run();
        }
    }
}