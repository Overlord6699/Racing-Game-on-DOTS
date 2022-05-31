using FMOD.Studio;
using Unity.Entities;

namespace Drift.Sounds
{
    public struct FMODSound : ISystemStateComponentData
    {
        public EventInstance Event;
    }

    public struct FMODFloatParameter : IBufferElementData
    {
        public PARAMETER_ID ParameterId;
        public float CurrentValue;
    }
}