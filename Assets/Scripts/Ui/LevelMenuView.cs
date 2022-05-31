using Cysharp.Threading.Tasks;
using Zenject;

namespace Drift.Ui
{
    public class LevelMenuView : BindingRoot, IInitializable
    {
        protected bool hidden;

        public void Initialize()
        {
            gameObject.SetActive(true);
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            hidden = true;
            var token = this.GetCancellationTokenOnDestroy();
            await GetComponent<VisualStateMachine>().GotoStateAsync("Initial");
            if (token.IsCancellationRequested) return;
            gameObject.SetActive(false);
        }

        public UniTask Show()
        {
            if (!hidden) return UniTask.CompletedTask;
            hidden = false;
            gameObject.SetActive(true);
            return GetComponent<VisualStateMachine>().GotoStateAsync("Open");
        }

        public async UniTask Hide()
        {
            if (hidden) return;
            hidden = true;
            var token = this.GetCancellationTokenOnDestroy();
            await GetComponent<VisualStateMachine>().GotoStateAsync("Closed");
            if (token.IsCancellationRequested || !hidden) return;
            gameObject.SetActive(false);
        }
    }
}