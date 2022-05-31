using Cysharp.Threading.Tasks;
using Drift.Ui;
using UnityEngine.InputSystem;

namespace Drift.States
{
    public class LevelPlayState : IState
    {
        private readonly IStateMachine<LevelTrigger> states;
        private readonly IInputService inputService;
        private readonly LevelPlayView levelPlayView;
        
        public readonly IObservableValue<VehicleInfo> Vehicle;

        public LevelPlayState(IStateMachine<LevelTrigger> states,
            IInputService inputService, IBridgeService bridgeService,
            LevelPlayView levelPlayView)
        {
            this.states = states;
            this.inputService = inputService;
            this.levelPlayView = levelPlayView;
            
            Vehicle = new ObservableValue<VehicleInfo>(bridgeService.Vehicle);
        }

        public void OnEnter()
        {
            inputService.Player.Enable();
            inputService.UI.ToggleMenu.performed += OnToggleMenuPerformed;
            
            levelPlayView.DataContext = this;
            levelPlayView.Show().Forget();
        }

        private void OnToggleMenuPerformed(InputAction.CallbackContext obj)
        {
            states.Fire(LevelTrigger.Menu);
        }

        public void OnExit()
        {
            inputService.UI.ToggleMenu.performed -= OnToggleMenuPerformed;
            levelPlayView.Hide().Forget();
        }
    }
}