using System;

namespace Drift.Ui
{
    public class StringFromAnyAdapter<T> : AdapterBase<T, string>
    {
        private readonly string format;

        public override string Value
        {
            get => string.IsNullOrEmpty(format) ? value.Value.ToString() : string.Format(format, value.Value);
            set => throw new NotSupportedException("Not supported operation");
        }

        public StringFromAnyAdapter(IValue<T> value, string format) : base(value)
        {
            this.format = format;
        }
    }

    public static class StringAdapterBuilder
    {
        public static IValue<string> AdaptToString(this IValue value, string format)
        {
            if (value is IValue<string> stringValue) return stringValue;
            var anyToStringAdapterType = typeof(StringFromAnyAdapter<>).MakeGenericType(value.Type);
            return (IValue<string>) Activator.CreateInstance(anyToStringAdapterType, value, format);
        }
    }
}