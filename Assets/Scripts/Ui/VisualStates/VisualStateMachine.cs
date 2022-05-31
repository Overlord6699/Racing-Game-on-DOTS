using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Drift.Ui
{
    public interface IVisualStateMachine
    {
        void GotoState(string stateName);
        UniTask GotoStateAsync(string stateName);
    }

    public class VisualStateMachine : MonoBehaviour, IVisualStateMachine
    {
        [SerializeField]
        private VisualStates states;
        [SerializeField]
        private VisualStatesTemplate template;
        [SerializeField]
        private bool forwardState;
        [SerializeField]
        private string initialState;

        private void Start()
        {
            if (string.IsNullOrEmpty(initialState)) return;
            GotoState(initialState);
        }

        public async UniTask GotoStateAsync(string stateName)
        {
            DOTween.Kill(gameObject);
            var states = template != null ? template.States : this.states;

            var tasks = ListPool<UniTask>.Get();
            if (states.TryGetState(stateName, out var state))
            {
                tasks.AddRange(state.BuildSequence(this));
            }
            
            if (forwardState)
            {
                foreach (var visualStateMachine 
                    in transform.GetFirstDescendants<IVisualStateMachine>())
                {
                    tasks.Add(visualStateMachine.GotoStateAsync(stateName));
                }
            }
            
            await UniTask.WhenAll(tasks);
            ListPool<UniTask>.Release(tasks);
        }

        public void GotoState(string stateName)
        {
            DOTween.Kill(gameObject);
            var states = template != null ? template.States : this.states;
            
            if (states.TryGetState(stateName, out var state))
            {
                foreach (var _ in state.BuildSequence(this)) { }
            }
            
            if (forwardState)
            {
                foreach (var visualStateMachine 
                    in transform.GetFirstDescendants<IVisualStateMachine>())
                {
                    visualStateMachine.GotoState(stateName);
                }
            }
        }
    }
}