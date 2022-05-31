using Drift.Sounds;
using Unity.Entities;
using Unity.Mathematics;

namespace Drift
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public class VehicleSoundSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities.ForEach((in VehicleInput input, in VehicleOutput output, in VehicleSound sound) =>
            {
                
                var parametersBuffer = GetBuffer<FloatParameter>(sound.Engine);
                if (parametersBuffer.Length < 2) return;
                
                var rotationParameter = parametersBuffer[0];
                rotationParameter.Value = math.lerp(rotationParameter.Value, 
                    math.saturate(output.EngineRotationRate * sound.RotationMultiplier * (input.Load > 0.01f ? 1 : 0)), 
                    deltaTime * sound.RotationChangeSpeed);
                parametersBuffer[0] = rotationParameter;
                
                var loadParameter = parametersBuffer[1];
                loadParameter.Value = math.lerp(loadParameter.Value, input.Load, 
                    deltaTime * sound.LoadChangeSpeed);
                parametersBuffer[1] = rotationParameter;
                
            }).Schedule();
        }
    }
}