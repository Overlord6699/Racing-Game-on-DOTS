using System;

namespace Drift.Ui
{
    public interface ICommand
    { 
        event Action<bool> IsEnabledChanged;
        bool IsEnabled { get; set; }
        void TryExecute();
    }
}