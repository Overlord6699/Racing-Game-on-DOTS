using System;

namespace Drift.Ui
{
    public class DelegateCommand : ICommand
    {
        private bool isEnabled = true;
        private Action action;
        
        public event Action<bool> IsEnabledChanged;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value) return;
                isEnabled = value;
                IsEnabledChanged?.Invoke(value);
            }
        }

        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        public void TryExecute()
        {
            if (IsEnabled) action?.Invoke();
        }
    }
}