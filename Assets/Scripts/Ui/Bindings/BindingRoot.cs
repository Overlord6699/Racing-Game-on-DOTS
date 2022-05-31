using UnityEngine;

namespace Drift.Ui
{
    public class BindingRoot : MonoBehaviour, IBinding
    {
        private object dataContext;
        
        public ForwardedBindingStrategy ForwardedBindingStrategy 
            => ForwardedBindingStrategy.Ignore;
        
        public object DataContext
        {
            get => dataContext;
            set
            {
                dataContext = value;
                this.SetDataContextRecursive(value);
            }
        }
    }
}