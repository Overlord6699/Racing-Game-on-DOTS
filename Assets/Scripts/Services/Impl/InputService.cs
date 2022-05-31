using System;

namespace Drift
{
    public class InputService : IInputService, IDisposable
    {
        private readonly Controls controls;

        public Controls.DebugActions Debug => controls.Debug;
        public Controls.PlayerActions Player => controls.Player;
        public Controls.UIActions UI => controls.UI;

        public InputService()
        {
            controls = new Controls();
            controls.Enable();
        }

        public void Dispose()
        {
            controls?.Dispose();
        }
    }
}