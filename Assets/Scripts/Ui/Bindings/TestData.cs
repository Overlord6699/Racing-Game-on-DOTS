using UnityEngine;

namespace Drift.Ui
{
    public class TestData
    {
        public readonly ObservableValue<string> String = new ObservableValue<string>("Hello");
        public readonly ObservableValue<float> Float = new ObservableValue<float>(1.234f);
        public readonly ObservableValue<int> Int = new ObservableValue<int>(543);
        public readonly ObservableValue<bool> Bool = new ObservableValue<bool>(false);

        public readonly ICommand Command1;
        public readonly ICommand Command2;

        public TestData()
        {
            Command1 = new DelegateCommand(() =>
            {
                String.Value = "Value" + Random.Range(1000, 10000);
            });
            Command2 = new DelegateCommand(() =>
            {
                Command1.IsEnabled = !Command1.IsEnabled;
            });
        }
    }
}