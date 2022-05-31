using Drift.Ui;

namespace Drift
{
    public interface IBridgeService
    {
        VehicleInfo Vehicle { get; }
    }

    public class VehicleInfo
    {
        public IObservableValue<float> Speed = new ObservableValue<float>(0);
        public IObservableValue<float> Throttle = new ObservableValue<float>(0);
        public IObservableValue<float> Angle = new ObservableValue<float>(0);
    }
}