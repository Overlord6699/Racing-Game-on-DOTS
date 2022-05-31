using System.Collections.Generic;
using Drift.Ui;
using UnityEngine;
using UnityEngine.Localization;

namespace Drift
{
    public interface IConfigurationService
    {
        public IGameConfiguration Game { get; }
        
        public ISoundConfiguration Sound { get; }
        
        public IGraphicsConfiguration Graphics { get; }
    }

    public interface IGameConfiguration
    {
        public IObservableValue<Locale> CurrentLocale { get; }
        
        public IEnumerable<Locale> AllLocales { get; }
    }

    public interface ISoundConfiguration
    {
        public IObservableValue<float> MainVolume { get; }
    }

    public interface IGraphicsConfiguration
    {
        public IObservableValue<Resolution> ActiveResolution { get; }
        
        public IEnumerable<Resolution> AllResolutions { get; }
    }
}