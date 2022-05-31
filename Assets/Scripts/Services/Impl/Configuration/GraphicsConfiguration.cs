using System.Collections.Generic;
using Drift.Ui;
using UnityEngine;

namespace Drift
{
    public class GraphicsConfiguration : IGraphicsConfiguration
    {
        public IObservableValue<Resolution> ActiveResolution { get; }
            = new ObservableValue<Resolution>(default);
        
        public IEnumerable<Resolution> AllResolutions { get; private set; }

        public void Initialize()
        {
            ActiveResolution.Value = Screen.currentResolution;
            AllResolutions = Screen.resolutions;
            
            ActiveResolution.ValueChanged += ActiveResolutionOnValueChanged;
        }

        private void ActiveResolutionOnValueChanged(Resolution resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, 
                Screen.fullScreenMode, resolution.refreshRate);
        }
    }
}