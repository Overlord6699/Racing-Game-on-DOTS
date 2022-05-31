namespace Drift.States
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }
}