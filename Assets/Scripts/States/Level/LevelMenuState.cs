using Cysharp.Threading.Tasks;
using Drift.Ui;

namespace Drift.States
{
    public class LevelMenuState : StateMachine<LevelMenuTrigger>, IState
    {
        private readonly IInputService inputService;
        private readonly IStateMachine<LevelTrigger> states;
        private readonly LevelMenuView levelMenuView;
        
        private readonly ICommand hideMenu;
        
        public readonly IObservableValue<LevelMenuMainState> Main = 
            new ObservableValue<LevelMenuMainState>(null);
        public readonly IObservableValue<LevelMenuSettingsState> Settings = 
            new ObservableValue<LevelMenuSettingsState>(null);
        public readonly IObservableValue<string> ActivePanel = 
            new ObservableValue<string>(null);

        public LevelMenuState(IInputService inputService,
            IStateMachine<LevelTrigger> states,
            IGameService gameService,
            IConfigurationService configurationService,
            LevelMenuView levelMenuView)
        {
            this.inputService = inputService;
            this.states = states;
            this.levelMenuView = levelMenuView;
            
            hideMenu = new DelegateCommand(HideMenu);
            
            Main.Value = new LevelMenuMainState(this, inputService, gameService, hideMenu);
            Settings.Value = new LevelMenuSettingsState(this, inputService, configurationService);
            Main.Value.IsEnabled.ValueChanged += OnMainEnabled;
            Settings.Value.IsEnabled.ValueChanged += OnSettingsEnabled;
            
            DefineState(() => Main.Value);
            DefineState(() => Settings.Value);
            
            DefineStartTransition<LevelMenuMainState>(LevelMenuTrigger.Main);
            DefineTransition<LevelMenuMainState, LevelMenuSettingsState>(LevelMenuTrigger.Settings);
            DefineTransition<LevelMenuSettingsState, LevelMenuMainState>(LevelMenuTrigger.Main);
            
            Fire(LevelMenuTrigger.Main);
        }

        private void OnSettingsEnabled(bool obj)
        {
            if (obj) ActivePanel.Value = nameof(Settings);
        }

        private void OnMainEnabled(bool obj)
        {
            if (obj) ActivePanel.Value = nameof(Main);
        }

        private void HideMenu()
        {
            states.Fire(LevelTrigger.Play);
            levelMenuView.Hide().Forget();
        }

        public void OnEnter()
        {
            inputService.Player.Disable();

            levelMenuView.DataContext = this;
            levelMenuView.Show().Forget();
        }

        public void OnExit()
        {
            Stop();
        }
    }

    public enum LevelMenuTrigger
    {
        Main,
        Settings
    }
}