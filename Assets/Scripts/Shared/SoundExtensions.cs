using Drift.Sounds;
using Unity.Entities;

namespace Drift
{
    public static class SoundExtensions
    {
        public static void MakeSound(this EntityManager dstManager, Entity entity,
            SoundDefinition soundDefinition, float volume = 1)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(Sound),
                typeof(Volume)
            ));
            
            dstManager.SetComponentData(entity, new Sound
            {
                Id = soundDefinition.Id
            });
            dstManager.SetComponentData(entity, new Volume
            {
                Value = volume
            });
            var parameters = soundDefinition.FloatParameters;
            if (parameters != null && parameters.Length > 0)
            {
                var floatParameters = dstManager.AddBuffer<FloatParameter>(entity);
                floatParameters.Capacity = parameters.Length;
                floatParameters.Length = parameters.Length;
                for (int i = 0; i < parameters.Length; i++)
                {
                    floatParameters[i] = new FloatParameter
                    {
                        Value = parameters[i].DefaultValue
                    };
                }
            }
        }
    }
}