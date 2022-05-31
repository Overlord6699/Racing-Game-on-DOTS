using UnityEngine;

namespace Drift.Ui
{
    public class BindingBase : MonoBehaviour, IBinding
    {
        private object dataContext;

        public virtual ForwardedBindingStrategy ForwardedBindingStrategy 
            => ForwardedBindingStrategy.AcceptAndForward;
        
        public object DataContext
        {
            get => dataContext;
            set => OnDataContextChanged(value);
        }
        
        protected virtual void OnDataContextChanged(object dataContext) { }
    }
}