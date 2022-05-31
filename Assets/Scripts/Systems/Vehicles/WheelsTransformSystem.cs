using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Drift.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class WheelsTransformSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref Rotation rotation, in WheelInput input,
                in WheelContact contact, in WheelOutput output) =>
            {

                var originTransform = input.LocalTransform;
                translation.Value = originTransform.pos - math.rotate(originTransform.rot, math.up()) * contact.Distance;
                rotation.Value = math.mul(originTransform.rot, quaternion.AxisAngle(math.right(), output.Rotation));

            }).Schedule();
        }
    }
}