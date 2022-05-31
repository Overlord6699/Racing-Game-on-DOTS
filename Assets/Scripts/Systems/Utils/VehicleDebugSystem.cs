#if UNITY_EDITOR

using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;

namespace Drift
{
    public class VehicleDebugSystem : SystemBase
    {
        private static readonly float updateInterval = 0.05f;
        private VehicleDebugWindow debugWindow;

        private float timeSincePreviousUpdate;

        protected override void OnUpdate()
        {
            timeSincePreviousUpdate += Time.DeltaTime;
            if (timeSincePreviousUpdate > updateInterval)
            {
                if (debugWindow == null)
                {
                    if (!EditorWindow.HasOpenInstances<VehicleDebugWindow>()) return;
                    debugWindow = EditorWindow.GetWindow<VehicleDebugWindow>();
                }
                timeSincePreviousUpdate = timeSincePreviousUpdate % updateInterval;
                Entities.WithAll<VehicleDebug>().ForEach((in Vehicle vehicle) =>
                {
                    for (int i = 0; i < vehicle.Wheels.Length; i++)
                    {
                        var wheelEntity = vehicle.Wheels[i];
                        var wheelInput = GetComponent<WheelInput>(wheelEntity);
                        var wheelOutput = GetComponent<WheelOutput>(wheelEntity);

                        debugWindow.SetWheel(i, new VehicleDebugWindow.WheelData
                        {
                            Impulse = wheelOutput.FrictionImpulse,
                            Torque = wheelInput.Torque,
                            RotationSpeed = wheelOutput.RotationSpeed,
                            SuspensionImpulse = math.length(wheelOutput.SuspensionImpulse),
                            Slip = wheelOutput.Slip
                        });
                    }
                }).WithoutBurst().Run();
                debugWindow.Repaint();
            }
        }
    }
}

#endif