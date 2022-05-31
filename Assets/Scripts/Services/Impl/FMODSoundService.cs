using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Drift
{
    [CreateAssetMenu(menuName = "Database/Sounds")]
    public class FMODSoundService : ScriptableObject, IFMODSoundService, IInitializable
    {
        [SerializeField] 
        private SoundDefinition[] sounds;

        public SoundDefinition[] Sounds => sounds;
        
        private SoundData[] soundDatas;
        
        public SoundDefinition GetSoundById(int id)
        {
            foreach (var soundDefinition in sounds)
            {
                if (soundDefinition.Id == id)
                {
                    return soundDefinition;
                }
            }
            throw new ApplicationException($"Sound with id {id} not found");
        }
        
        public EventDescription GetEventDefinition(SoundDefinition soundDefinition)
        {
            var soundData = soundDatas[soundDefinition.RuntimeIndex];
            if (!soundData.EventDefinition.isValid()) 
                throw new ApplicationException($"Event {sounds[soundDefinition.RuntimeIndex].Name} not valid");
            return soundData.EventDefinition;
        }
        
        public PARAMETER_DESCRIPTION GetFloatParameterDescription(
            SoundDefinition soundDefinition, FloatParameterDefinition parameter)
        {
            var soundData = soundDatas[soundDefinition.RuntimeIndex];
            
            var parameterIndex = parameter.RuntimeIndex;
            if (soundData.FloatParameterDefinitions.Length <= parameterIndex) 
                throw new ApplicationException($"Event {sounds[soundDefinition.RuntimeIndex].Name} don't " +
                                               $"have {parameterIndex + 1} parameters");

            var parameterDefinition = soundData.FloatParameterDefinitions[parameterIndex];
            return parameterDefinition;
        }
        
        public void Initialize()
        {
            soundDatas = new SoundData[sounds.Length];
            for (var index = 0; index < sounds.Length; index++)
            {
                var soundDefinition = sounds[index];
                
                soundDefinition.RuntimeIndex = index;
                
                ref var data = ref soundDatas[index];
                
                var result = RuntimeManager.StudioSystem.getEvent(soundDefinition.Name, out data.EventDefinition);
                if (result != RESULT.OK)
                {
                    Debug.LogError($"Event \"{soundDefinition.Name}\" not found");
                    continue;
                }
                data.FloatParameterDefinitions = new PARAMETER_DESCRIPTION[soundDefinition.FloatParameters.Length];
                for (int i = 0; i < soundDefinition.FloatParameters.Length; i++)
                {
                    ref var parameterDefinition = ref data.FloatParameterDefinitions[i];
                    result = data.EventDefinition.getParameterDescriptionByName(
                        soundDefinition.FloatParameters[i].Name, out parameterDefinition);
                    if (result != RESULT.OK)
                    {
                        Debug.LogError($"Event \"{soundDefinition.Name}\" parameter \"{soundDefinition.FloatParameters[i].Name}\" not found");
                    }

                    soundDefinition.FloatParameters[i].RuntimeIndex = i;
                }
            }
        }
        
        private struct SoundData
        {
            public EventDescription EventDefinition;
            public PARAMETER_DESCRIPTION[] FloatParameterDefinitions;
        }
    }
}