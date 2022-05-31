using TMPro;
using UnityEngine;

namespace Drift.Ui
{
    public class TextBinding : PropertyBindingBase<string>
    {
        private TextMeshProUGUI text;
        [SerializeField]
        public string format;
        
        protected override void OnPropertyValueChanged(string newValue)
        {
            text.text = newValue;
        }
        
        protected override void OnDataContextChanged(object dataContext)
        {
            if (text == null) text = GetComponent<TextMeshProUGUI>();
            base.OnDataContextChanged(dataContext);
        }

        protected override IValue<string> GetProperty(object dataContext, string propertyName)
        {
            return dataContext.GetProperty(propertyName).AdaptToString(format);
        }
    }
}