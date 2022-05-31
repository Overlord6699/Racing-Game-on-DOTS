using System;
using Drift.Ui;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Drift
{
    [Serializable]
    public class FmodSoundConfiguration : ISoundConfiguration
    {
        private Bus mainBus;
        [SerializeField]
        private ObservableValue<float> mainVolume = new ObservableValue<float>(0.5f);

        public IObservableValue<float> MainVolume => mainVolume;

        public void Initialize()
        {
            mainBus = RuntimeManager.GetBus("bus:/");
            MainVolume.ValueChanged += VolumeOnValueChanged;
            VolumeOnValueChanged(MainVolume.Value);
        }

        private void VolumeOnValueChanged(float volume)
        {
            mainBus.setVolume(volume);
        }
    }
}