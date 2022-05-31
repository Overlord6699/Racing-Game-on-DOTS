using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Drift.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public class VehicleToWheelsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref VehicleOutput output, in Vehicle vehicle, in Translation translation, in Rotation rotation, in PhysicsMass mass, 
                in VehicleInput input, in VehicleEngine engine) =>
            {
                var toEngineRpm = engine.TransmitWheelRotationSpeedToEngineRpm(
                    math.abs(output.MaxWheelRotationSpeed));
                output.EngineRotationRate = toEngineRpm / engine.Torque.Value.TimeRange.y;
                var engineTorque = engine.EvaluateTorque(toEngineRpm) * input.Throttle;
                var wheelsTorque = engine.TransmitEngineTorqueToWheelTorque(engineTorque);
                
                for (int i = 0; i < vehicle.Wheels.Length; i++)
                {
                    var wheelEntity = vehicle.Wheels[i];
                    var origin = GetComponent<WheelOrigin>(wheelEntity);
                    var controllable = GetComponent<WheelControllable>(wheelEntity);

                    var localTransform = origin.Value;
                    localTransform.rot = math.mul(localTransform.rot,
                        quaternion.AxisAngle(math.up(), input.Steering * controllable.MaxSteeringAngle));

                    var wheelTransform = math.mul(new RigidTransform(rotation.Value, translation.Value), localTransform);
                    
                    var wheelInput = new WheelInput
                    {
                        LocalTransform = localTransform,
                        WorldTransform = wheelTransform,
                        Up = math.rotate(wheelTransform.rot, math.up()),
                        MassMultiplier = 1.0f / mass.InverseMass,
                        Torque = wheelsTorque * controllable.DriveRate,
                        Brake = controllable.BrakeRate * input.Brake,
                        Handbrake = controllable.HandbrakeRate * input.Handbrake
                    };
                    SetComponent(wheelEntity, wheelInput);

                    if (controllable.DriveRate > 0)
                    {
                        var wheelOutput = GetComponent<WheelOutput>(wheelEntity);
                        wheelOutput.RotationSpeed = output.AverageWheelRotationSpeed;
                        SetComponent(wheelEntity, wheelOutput);
                    }
                }
            }).Schedule();
        }
    }
}