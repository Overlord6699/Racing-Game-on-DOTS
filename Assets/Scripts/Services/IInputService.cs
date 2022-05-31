namespace Drift
{
    public interface IInputService
    {
        public Controls.DebugActions Debug { get; }
        public Controls.PlayerActions Player { get; }
        public Controls.UIActions UI { get; }
    }
}