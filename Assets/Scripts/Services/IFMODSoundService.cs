using FMOD.Studio;

namespace Drift
{
    public interface IFMODSoundService : ISoundService
    {
        EventDescription GetEventDefinition(SoundDefinition soundDefinition);

        PARAMETER_DESCRIPTION GetFloatParameterDescription(SoundDefinition soundDefinition,
            FloatParameterDefinition parameter);
    }
}