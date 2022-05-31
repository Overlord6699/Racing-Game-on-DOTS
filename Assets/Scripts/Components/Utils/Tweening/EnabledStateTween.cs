using Unity.Entities;

namespace Drift.Tweening
{
    public struct EnabledStateTween : IComponentData
    {
        public bool State;
    }
}