using UnityEngine;
using Zenject;

namespace Drift
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField]
        private Database database;
        
        [SerializeField]
        private FMODSoundService soundService;
        
        [SerializeField]
        private ConfigurationService configuration;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InputService>()
                .AsSingle()
                .Lazy();

            Container.BindInterfacesTo<GameService>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<FMODSoundService>()
                .FromInstance(soundService)
                .AsSingle();

            Container.BindInterfacesTo<Database>()
                .FromInstance(database)
                .AsSingle();
            
            Container.BindInterfacesTo<ConfigurationService>()
                .FromInstance(configuration)
                .AsSingle();
        }
    }
}