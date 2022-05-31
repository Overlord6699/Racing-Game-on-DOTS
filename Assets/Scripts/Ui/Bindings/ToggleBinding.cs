using UnityEngine;
using UnityEngine.UI;

namespace Drift.Ui
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleBinding : PropertyBindingBase<bool>
    {
        private Toggle toggle;

        protected override void OnDataContextChanged(object dataContext)
        {
            if (toggle == null)
            {
                toggle = GetComponent<Toggle>();
                toggle.onValueChanged.AddListener(OnToggleValueChanged);
            }
            base.OnDataContextChanged(dataContext);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool newValue)
        {
            UpdateSourceValue(newValue);
        }

        protected override void OnPropertyValueChanged(bool newValue)
        {
            toggle.isOn = newValue;
        }
    }
}