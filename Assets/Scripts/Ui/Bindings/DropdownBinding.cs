using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Drift.Ui
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownBinding : PropertyBindingBase<object>
    {
        [SerializeField] 
        private string collectionName;
        
        private IList collection;

        private TMP_Dropdown dropdown;

        protected override void OnDataContextChanged(object dataContext)
        {
            if (dropdown == null)
            {
                dropdown = GetComponent<TMP_Dropdown>();
                dropdown.onValueChanged.AddListener(OnValueChanged);
            }
            
            dropdown.options.Clear();
            
            if (dataContext == null || string.IsNullOrEmpty(collectionName))
            {
                dropdown.RefreshShownValue();
                UpdateSourceValue(null);
                return;
            }
            
            collection = dataContext.GetPublicFieldOrPropertyValue<IList>(collectionName);
            
            if (collection != null)
            {
                var options = new List<TMP_Dropdown.OptionData>(collection.Count);
                foreach (var o in collection)
                {
                    options.Add(new TMP_Dropdown.OptionData(o.ToString()));
                }
                dropdown.options = options;
            }
            
            base.OnDataContextChanged(dataContext);
        }

        private void OnValueChanged(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex > collection.Count)
                UpdateSourceValue(null);
            else
                UpdateSourceValue(collection[selectedIndex]);
        }

        protected override void OnPropertyValueChanged(object newValue)
        {
            if (collection == null) return;
            dropdown.value = collection.IndexOf(newValue);
        }
        
        protected override IValue<object> GetProperty(object dataContext, string propertyName)
        {
            return dataContext.GetProperty(propertyName).AdaptToObject();
        }
    }
}