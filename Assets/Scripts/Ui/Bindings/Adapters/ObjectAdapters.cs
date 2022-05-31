using System;

namespace Drift.Ui
{
    public sealed class ObjectFromAnyAdapter<T> : AdapterBase<T, object>
    {
        public override object Value
        {
            get => value.Value;
            set => this.value.Value = (T) value;
        }

        public ObjectFromAnyAdapter(IValue<T> value) : base(value)
        {
        }
    }
    
    public static class ObjectAdapterBuilder
    {
        public static IValue<object> AdaptToObject(this IValue value)
        {
            var anyToStringAdapterType = typeof(ObjectFromAnyAdapter<>).MakeGenericType(value.Type);
            return (IValue<object>) Activator.CreateInstance(anyToStringAdapterType, value);
        }
    }
}