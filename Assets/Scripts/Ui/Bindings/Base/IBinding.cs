using System;

namespace Drift.Ui
{
    public interface IBinding
    {
        ForwardedBindingStrategy ForwardedBindingStrategy { get; }
        object DataContext { get; set; }
    }

    [Flags]
    public enum ForwardedBindingStrategy : byte
    {
        Ignore = 0b00,
        Accept = 0b01,
        Forward = 0b10,
        AcceptAndForward = 0b11
    }
}