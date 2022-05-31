using System.Linq;
using Drift.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

namespace Drift.States
{
    public class LevelMenuSettingsState : IState
    {
        private readonly IInputService inputService;
        
        public readonly ICommand Back;
        
        public IObservableValue<bool> IsEnabled = new ObservableValue<bool>(false);
        
        public readonly IObservableValue<Locale> ActiveLocale;
        public Locale[] AvailableLocales;
        
        public readonly IObservableValue<Resolution> ActiveResolution;
        public Resolution[] AvailableResolutions;
        
        public readonly IObservableValue<float> MainVolume;
        
        public LevelMenuSettingsState(IStateMachine<LevelMenuTrigger> stateMachine,
            IInputService inputService, IConfigurationService configurationService)
        {
            this.inputService = inputService;
            Back = new DelegateCommand(() => stateMachine.Fire(LevelMenuTrigger.Main));

            ActiveLocale = configurationService.Game.CurrentLocale;
            ActiveResolution = configurationService.Graphics.ActiveResolution;
            MainVolume = configurationService.Sound.MainVolume;

            AvailableLocales = configurationService.Game.AllLocales.ToArray();
            AvailableResolutions = configurationService.Graphics.AllResolutions.ToArray();
        }
        
        public void OnEnter()
        {
            IsEnabled.Value = true;
            inputService.UI.ToggleMenu.performed += OnToggleMenuPerformed;
        }

        private void OnToggleMenuPerformed(InputAction.CallbackContext obj)
        {
            Back.TryExecute();
        }

        public void OnExit()
        {
            IsEnabled.Value = false;
            inputService.UI.ToggleMenu.performed -= OnToggleMenuPerformed;
        }
    }
}