using System;

namespace Drift.Ui
{
    public class FloatFromAnyAdapter<T> : AdapterBase<T, float>
    {
        public FloatFromAnyAdapter(IValue<T> value) : base(value)
        {
        }

        public override float Value
        {
            get => Convert.ToSingle(value.Value);
            set => this.value.Value = (T)Convert.ChangeType(value, typeof(T));
        }
    }

    public class FloatFromIntAdapter : AdapterBase<int, float>
    {
        public FloatFromIntAdapter(IValue<int> value) : base(value)
        {
        }

        public override float Value
        {
            get => this.value.Value;
            set => this.value.Value = (int) value;
        }
    }

    public static class FloatAdapterBuilder
    {
        public static IValue<float> AdaptToFloat(this IValue value)
        {
            if (value.Type == typeof(float)) return (IValue<float>) value;
            if (value.Type == typeof(int)) return new FloatFromIntAdapter((IValue<int>)value);
            var anyToStringAdapterType = typeof(FloatFromAnyAdapter<>).MakeGenericType(value.Type);
            return (IValue<float>) Activator.CreateInstance(anyToStringAdapterType, value);
        }
    }
}