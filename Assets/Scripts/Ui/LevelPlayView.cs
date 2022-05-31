using Cysharp.Threading.Tasks;

namespace Drift.Ui
{
    public class LevelPlayView : BindingRoot
    {
        protected bool hidden;
        
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