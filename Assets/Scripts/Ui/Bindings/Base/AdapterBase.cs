using System;

namespace Drift.Ui
{
    public abstract class AdapterBase<TSource, TDest> : IObservableValue<TDest>
    {
        protected readonly IValue<TSource> value;

        public abstract TDest Value { get; set; }

        public Type Type => typeof(TDest);

        protected AdapterBase(IValue<TSource> value)
        {
            this.value = value;
        }

        public event Action<TDest> ValueChanged
        {
            add
            {
                valueChanged += value;
                if (this.value is IObservableValue<TSource> observableValue)
                {
                    observableValue.ValueChanged -= ValueOnValueChanged;
                    observableValue.ValueChanged += ValueOnValueChanged;
                }
            }
            remove
            {
                valueChanged -= value;
                if (valueChanged == null
                    && this.value is IObservableValue<TSource> observableValue)
                {
                    observableValue.ValueChanged -= ValueOnValueChanged;
                }
            }
        }

        private Action<TDest> valueChanged;

        private void ValueOnValueChanged(TSource obj)
        {
            valueChanged?.Invoke(Value);
        }
    }
}