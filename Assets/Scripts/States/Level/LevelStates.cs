using Drift.Ui;
using Unity.Entities;

namespace Drift.States
{
    public class LevelStates : StateMachine<LevelTrigger>
    {
        public LevelStates(World world, IInputService inputService, 
            IGameService gameService, IBridgeService bridgeService, 
            IConfigurationService configurationService,
            LevelPlayView levelPlayView, LevelMenuView levelMenuView)
        {
            DefineState(() => new LevelPrepareState(this, world));
            DefineState(() => new LevelPlayState(this, inputService, bridgeService, levelPlayView));
            DefineState(() => new LevelMenuState(inputService, this, gameService, configurationService, levelMenuView));
            
            DefineStartTransition<LevelPrepareState>(LevelTrigger.Play);
            DefineTransition<LevelPrepareState, LevelPlayState>(LevelTrigger.Play);
            DefineTransition<LevelPlayState, LevelMenuState>(LevelTrigger.Menu);
            DefineTransition<LevelMenuState, LevelPlayState>(LevelTrigger.Play);
        }
    }
    
    public enum LevelTrigger
    {
        Menu,
        Play
    }
}