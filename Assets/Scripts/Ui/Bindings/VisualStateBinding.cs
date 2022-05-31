using UnityEngine;

namespace Drift.Ui
{
    [RequireComponent(typeof(VisualStateMachine))]
    public class VisualStateBinding : PropertyBindingBase<string>
    {
        private VisualStateMachine visualStateMachine;
        
        protected override void OnDataContextChanged(object dataContext)
        {
            if (visualStateMachine == null) visualStateMachine = GetComponent<VisualStateMachine>();
            base.OnDataContextChanged(dataContext);
        }
        
        protected override void OnPropertyValueChanged(string newValue)
        {
            visualStateMachine.GotoState(newValue);
        }

        protected override IValue<string> GetProperty(object dataContext, string propertyName)
        {
            return dataContext.GetProperty(propertyName).AdaptToString(null);
        }
    }
}