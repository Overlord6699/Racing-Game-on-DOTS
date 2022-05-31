using Drift.States;
using Drift.Ui;
using UnityEngine;
using Zenject;

namespace Drift
{
    public class LevelSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private SurfaceService surfaces;
        [SerializeField]
        private LevelMenuView LevelMenuView;
        [SerializeField]
        private LevelPlayView LevelPlayView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SurfaceService>()
                .FromInstance(surfaces)
                .AsSingle();
            
            Container.BindInterfacesTo<BridgeService>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelMenuView>()
                .FromInstance(LevelMenuView)
                .AsSingle();
            Container.BindInterfacesAndSelfTo<LevelPlayView>()
                .FromInstance(LevelPlayView)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<LevelStates>()
                .AsSingle();
        }

        public override void Start()
        {
            Container.Resolve<LevelStates>().Fire(LevelTrigger.Play);
        }
    }
}