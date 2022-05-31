using UnityEngine;

namespace Drift.Ui
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InteractableBinding : PropertyBindingBase<bool>
    {
        private CanvasGroup canvasGroup;

        protected override void OnDataContextChanged(object dataContext)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            base.OnDataContextChanged(dataContext);
        }

        protected override void OnPropertyValueChanged(bool newValue)
        {
            canvasGroup.interactable = newValue;
        }
    }
}