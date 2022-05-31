using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Drift
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(WheelSkidmarksSystem))]
    public class SkidmarksLifetimeSystem : SystemBase
    {
        private RemoveSkidmarksSystem removeSkidmarksSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            removeSkidmarksSystem = World.GetExistingSystem<RemoveSkidmarksSystem>();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();

            Entities.ForEach((Entity entity, in Skidmarks skidmarks, in RenderMesh renderMesh) =>
            {
                Object.Destroy(renderMesh.mesh);
            }).WithoutBurst().Run();
        }
        
        protected override void OnUpdate()
        {
            var requests = removeSkidmarksSystem.CreateBuffer();
            var deltaTime = Time.DeltaTime;
            Dependency = Entities.WithAll<Skidmarks>().ForEach((Entity entity, ref Lifetime lifetime) =>
            {
                lifetime.Time -= deltaTime;
                if (lifetime.Time < 0) requests.Enqueue(new RemoveSkidmarksSystem.Request {Entity = entity});
            }).Schedule(Dependency);
            removeSkidmarksSystem.AddProducerJob(Dependency);
        }
    }
}