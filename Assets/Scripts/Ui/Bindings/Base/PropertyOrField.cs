using System;
using System.Reflection;

namespace Drift.Ui
{
    public readonly struct PropertyOrField
    {
        private readonly PropertyInfo propertyInfo;
        private readonly FieldInfo fieldInfo;
        
        public Type Type => propertyInfo != null ? propertyInfo.PropertyType : fieldInfo.FieldType;
        
        public bool IsOfType(Type type)
        {
            var currentType = Type;
            return currentType == type || type.IsAssignableFrom(currentType);
        }
        
        public PropertyOrField(PropertyInfo propertyInfo) : this()
        {
            this.propertyInfo = propertyInfo;
        }

        public PropertyOrField(FieldInfo fieldInfo) : this()
        {
            this.fieldInfo = fieldInfo;
        }
        
        public T GetValue<T>(object container)
        {
            return (T) (propertyInfo != null
                ? propertyInfo.GetValue(container)
                : fieldInfo.GetValue(container));
        }
        
        public void SetValue<T>(object container, T value)
        {
            if (propertyInfo != null)
                propertyInfo.SetValue(container, value);
            else
                fieldInfo.SetValue(container, value);
        }
    }
}