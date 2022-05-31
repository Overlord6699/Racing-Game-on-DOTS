using UnityEngine.UI;

namespace Drift.Ui
{
    public class AutoFocusBinding : PropertyBindingBase<bool>
    {
        protected override void OnPropertyValueChanged(bool newValue)
        {
            if (newValue)
            {
                GetComponentInChildren<Selectable>()?.Select();
            }
        }
    }
}