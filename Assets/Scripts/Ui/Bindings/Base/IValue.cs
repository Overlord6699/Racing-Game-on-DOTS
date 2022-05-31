using System;

namespace Drift.Ui
{
    public interface IValue
    {
        Type Type { get; }
    }

    public interface IValue<T> : IValue
    {
        public T Value { get; set; }
    }

    public interface IObservableValue<T> : IValue<T>
    {
        public event Action<T> ValueChanged;
    }
}