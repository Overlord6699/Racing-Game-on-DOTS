using System;
using Unity.Assertions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Drift.Ui
{
    public class MainMenuView : MonoBehaviour
    {
        public event UnityAction Play
        {
            add
            {
                Assert.IsNotNull(playButton, "Play button not defined");
                playButton.onClick.AddListener(value);
            }
            remove
            {
                Assert.IsNotNull(playButton, "Play button not defined");
                playButton.onClick.RemoveListener(value);
            }
        }
        
        [SerializeField]
        private Button playButton;
    }
}