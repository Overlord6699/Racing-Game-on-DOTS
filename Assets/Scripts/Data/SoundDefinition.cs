using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Drift
{
    [CreateAssetMenu(menuName = "Database/Sound")]
    public class SoundDefinition : ScriptableObject
    {
        [ReadOnlyInInspector]
        public int Id;
        public string Name;
        public FloatParameterDefinition[] FloatParameters;
        
        [NonSerialized]
        public int RuntimeIndex;
        
#if UNITY_EDITOR
        private void Reset()
        {
            var sounds = AssetDatabase.FindAssets("t:SoundDefinition");
            var firstAvailableId = 1;
            foreach (var soundGuid in sounds)
            {
                var sound = AssetDatabase.LoadAssetAtPath<SoundDefinition>(
                    AssetDatabase.GUIDToAssetPath(soundGuid));
                if (sound.Id >= firstAvailableId)
                    firstAvailableId = sound.Id + 1;
            }

            Id = firstAvailableId;
        }
#endif
    }
    
    [Serializable]
    public class FloatParameterDefinition
    {
        public string Name;
        public float DefaultValue;
        
        [NonSerialized]
        public int RuntimeIndex;
    }
}