using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Drift
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public class WheelSkidmarksSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var time = UnityEngine.Time.time;
            Entities.ForEach((Entity entity, in WheelSkidmarks skidmarks, in Surface wheelOnSurface,
                in WheelContact contact, in WheelOutput output, in Wheel wheel, in Rotation rotation,
                in WheelContactVelocity velocity) =>
            {

                var lastSequence = new Sequences(
                    GetBuffer<SkidmarksPoint>(skidmarks.ActiveSkidmarks),
                    GetBuffer<SkidmarksSequence>(skidmarks.ActiveSkidmarks));

                if (!contact.IsInContact)
                {
                    lastSequence.Complete();
                    return;
                }

                var activeSkidmarks = GetComponent<Skidmarks>(skidmarks.ActiveSkidmarks);
                var intenstity = math.saturate(math.unlerp(activeSkidmarks.SlipToIntensityRemap.x,
                    activeSkidmarks.SlipToIntensityRemap.y, output.Slip));
                if (intenstity < 0.01f)
                {
                    lastSequence.Complete();
                }
                else
                {
                    var contactVelocity = velocity.Value.ProjectOnPlane(contact.Normal);
                    var direction = math.normalizesafe(contactVelocity);
                    var right = math.cross(contact.Normal, direction);
                    var point = new SkidmarksPoint
                    {
                        Position = contact.Point,
                        Normal = contact.Normal,
                        Intensity = intenstity,
                        Right = right,
                        Width = wheel.Width,
                        CreationTime = time
                    };
                    lastSequence.Continue(point);
                }

            }).Run();//Schedule();
        }

        private struct Sequences
        {
            private DynamicBuffer<SkidmarksPoint> points;
            private DynamicBuffer<SkidmarksSequence> sequences;
            private int to;
            private int length;
            
            public void Complete()
            {
                if (length == 1)
                {
                    points.Length -= length;
                }
                else if (length > 1)
                {
                    sequences.Add(new SkidmarksSequence
                    {
                        End = to
                    });
                }
            }

            public void Continue(SkidmarksPoint point)
            {
                const float maxDistanceBetweenPointsSq = 0.1f * 0.1f;
                if (length > 1)
                {
                    points[to] = point;
                    if (math.distancesq(points[to - 1].Position, point.Position) > maxDistanceBetweenPointsSq)
                    {
                        points.Add(point);
                    }
                }
                else if (length == 1)
                {
                    if (math.distancesq(points[to].Position, point.Position) > maxDistanceBetweenPointsSq)
                    {
                        points.Add(point);
                    }
                }
                else
                {
                    points.Add(point);
                }
            }

            public Sequences(DynamicBuffer<SkidmarksPoint> points, DynamicBuffer<SkidmarksSequence> sequences)
            {
                this.points = points;
                this.sequences = sequences;
                var @from = sequences.Length > 0 ? sequences[sequences.Length - 1].End + 1 : 0;
                to = points.Length - 1;
                length = to - @from + 1;
            }
        }
    }
}