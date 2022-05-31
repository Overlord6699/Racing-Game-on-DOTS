using System;
using UnityEngine;
using UnityEngine.UI;

namespace Drift.Ui
{
    [RequireComponent(typeof(Button))]
    public class ButtonBinding : BindingBase
    {
        [SerializeField]
        private string commandName;
        
        private ICommand command;
        private Button button;

        protected override void OnDataContextChanged(object dataContext)
        {
            if (button == null) 
                Initialize();
            
            if (command != null) 
                command.IsEnabledChanged -= OnCommandIsEnabledChanged;
            
            if (dataContext == null || string.IsNullOrEmpty(commandName))
            {
                OnCommandIsEnabledChanged(false);
                return;
            }
            
            try
            {
                command = dataContext.GetPublicFieldOrPropertyValue<ICommand>(commandName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                command = null;
            }
            
            if (command != null)
            {
                command.IsEnabledChanged += OnCommandIsEnabledChanged;
                OnCommandIsEnabledChanged(command.IsEnabled);
            }
            else
            {
                OnCommandIsEnabledChanged(false);
            }
        }
        
        private void OnCommandIsEnabledChanged(bool isEnabled)
        {
            button.interactable = isEnabled;
        }

        private void Initialize()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        
        private void OnDestroy()
        {
            if (button == null) return;
            button.onClick.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            command?.TryExecute();
        }
    }
}