using Unity.Entities;
using Unity.Mathematics;

namespace Drift.Sounds
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class FMODUpdateFloatParametersSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithChangeFilter<FloatParameter>().ForEach((
                DynamicBuffer<FMODFloatParameter> fmodParameters,
                in FMODSound sound,
                in DynamicBuffer<FloatParameter> parameters
            ) =>
            {
                
                var length = math.min(fmodParameters.Length, parameters.Length);
                for (int i = 0; i < length; i++)
                {
                    var floatParameter = parameters[i];
                    var fmodFloatParameter = fmodParameters[i];
                    if (fmodFloatParameter.CurrentValue == floatParameter.Value) continue;
                    sound.Event.setParameterByID(fmodFloatParameter.ParameterId, floatParameter.Value);
                    
                    fmodFloatParameter.CurrentValue = floatParameter.Value;
                    fmodParameters[i] = fmodFloatParameter;
                }
                
            }).WithoutBurst().Run();
        }
    }
}