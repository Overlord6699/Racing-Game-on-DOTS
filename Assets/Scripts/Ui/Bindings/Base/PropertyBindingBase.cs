using System;
using UnityEngine;

namespace Drift.Ui
{
    public abstract class PropertyBindingBase<T> : BindingBase
    {
        private IValue<T> value;
        [SerializeField]
        private string propertyName;
        
        protected virtual IValue<T> GetProperty(object dataContext, string propertyName)
        {
            return dataContext.GetProperty<T>(propertyName);
        }

        protected override void OnDataContextChanged(object dataContext)
        {
            if (value is IObservableValue<T> observableValue1) 
                observableValue1.ValueChanged -= OnPropertyValueChanged;
            
            if (dataContext == null || string.IsNullOrEmpty(propertyName))
            {
                OnPropertyValueChanged(default);
                return;
            }
            
            try
            {
                value = GetProperty(dataContext, propertyName);
                
                if (value is IObservableValue<T> observableValue2) 
                    observableValue2.ValueChanged += OnPropertyValueChanged;
                OnPropertyValueChanged(value.Value);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                value = null;
                OnPropertyValueChanged(default);
            }
        }

        protected abstract void OnPropertyValueChanged(T newValue);
        
        protected void UpdateSourceValue(T newValue)
        {
            if (value != null) value.Value = newValue;
        }
    }
}