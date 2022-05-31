using TMPro;
using UnityEngine;

namespace Drift.Ui
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldBinding : PropertyBindingBase<string>
    {
        private TMP_InputField inputField;

        protected override void OnDataContextChanged(object dataContext)
        {
            if (inputField == null)
            {
                inputField = GetComponent<TMP_InputField>();
                inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
            }
            base.OnDataContextChanged(dataContext);
        }

        private void OnDestroy()
        {
            inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
        }

        private void OnInputFieldValueChanged(string arg0)
        {
            UpdateSourceValue(arg0);
        }
        
        protected override void OnPropertyValueChanged(string newValue)
        {
            inputField.text = newValue;
        }
    }
}