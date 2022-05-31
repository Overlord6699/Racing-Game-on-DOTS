using Drift.Ui;
using UnityEngine.InputSystem;

namespace Drift.States
{
    public class LevelMenuMainState : IState
    {
        private readonly IInputService inputService;
        
        public readonly ICommand GotoSettings;
        public readonly ICommand GotoMainMenu;
        public readonly ICommand HideMenu;
        
        public IObservableValue<bool> IsEnabled = new ObservableValue<bool>(false);

        public LevelMenuMainState(IStateMachine<LevelMenuTrigger> stateMachine,
            IInputService inputService,
            IGameService gameService, ICommand hideMenu)
        {
            this.inputService = inputService;
            GotoSettings = new DelegateCommand(() => stateMachine.Fire(LevelMenuTrigger.Settings));
            GotoMainMenu = new DelegateCommand(() => gameService.Fire(GameTrigger.MainMenu));
            HideMenu = hideMenu;
        }

        public void OnEnter()
        {
            inputService.UI.ToggleMenu.performed += OnToggleMenuPerformed;
            IsEnabled.Value = true;
        }

        private void OnToggleMenuPerformed(InputAction.CallbackContext obj)
        {
            HideMenu.TryExecute();
        }

        public void OnExit()
        {
            inputService.UI.ToggleMenu.performed -= OnToggleMenuPerformed;
            IsEnabled.Value = false;
        }
    }
}