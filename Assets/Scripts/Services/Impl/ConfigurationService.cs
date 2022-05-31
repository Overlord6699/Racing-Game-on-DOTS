using System;
using System.IO;
using UnityEngine;
using Zenject;

namespace Drift
{
    [CreateAssetMenu(menuName = "Game Configuration")]
    public class ConfigurationService : ScriptableObject, IConfigurationService, 
        IInitializable, IDisposable
    {
        [SerializeField]
        private string configurationFilePath = "configuration.json";
        
        private ClientConfiguration clientConfiguration;

        public IGameConfiguration Game => clientConfiguration.GameConfiguration;
        
        public ISoundConfiguration Sound => clientConfiguration.SoundConfiguration;
        
        public IGraphicsConfiguration Graphics => clientConfiguration.GraphicsConfiguration;
        
        [Serializable]
        private class ClientConfiguration
        {
            [SerializeField]
            public GameConfiguration GameConfiguration = new GameConfiguration();
            [SerializeField]
            public FmodSoundConfiguration SoundConfiguration = new FmodSoundConfiguration();
            [SerializeField]
            public GraphicsConfiguration GraphicsConfiguration = new GraphicsConfiguration();
        }

        public void Initialize()
        {
            var filePath = Path.Combine(Application.persistentDataPath, configurationFilePath);
            
            try
            {
                clientConfiguration = 
                    JsonUtility.FromJson<ClientConfiguration>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                clientConfiguration = new ClientConfiguration();
            }
            
            clientConfiguration.GameConfiguration.Initialize().Forget();
            clientConfiguration.SoundConfiguration.Initialize();
            clientConfiguration.GraphicsConfiguration.Initialize();
        }

        public void Dispose()
        {
            var filePath = Path.Combine(Application.persistentDataPath, configurationFilePath);
            try
            {
                File.WriteAllText(filePath, JsonUtility.ToJson(clientConfiguration));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}