using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Drift
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class VehicleBridgeSystem : SystemBase
    {
        private IBridgeService bridgeService;

        [Inject]
        private void Inject(IBridgeService bridgeService)
        {
            this.bridgeService = bridgeService;
        }
        
        protected override void OnUpdate()
        {
            var vehicleInfo = bridgeService.Vehicle;
            var deltaTime = Time.DeltaTime;
            Entities.WithAll<AttachedPlayer>().ForEach((Vehicle vehicle, 
                VehicleInput input, VehicleOutput output) =>
            {
                
                vehicleInfo.Throttle.Value = math.abs(input.Throttle);
                vehicleInfo.Speed.Value = math.length(output.LocalVelocity);
                var targetAngle = vehicleInfo.Speed.Value > 1f
                    ? math.radians(Vector3.SignedAngle(math.forward(), output.LocalVelocity, math.up()))
                    : 0.0f;
                if (math.abs(targetAngle) > math.PI * 0.5f)
                    targetAngle = 0.0f;
                vehicleInfo.Angle.Value = math.lerp(vehicleInfo.Angle.Value, targetAngle, deltaTime * 10);
                
            }).WithoutBurst().Run();
        }
    }
}