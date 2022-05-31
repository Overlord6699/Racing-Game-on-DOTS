using Unity.Entities;

namespace Drift
{
    public class WorldLifetimeSystem : SystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            ScriptBehaviourUpdateOrder.AddWorldToCurrentPlayerLoop(World);
            Enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (World.DefaultGameObjectInjectionWorld == World) 
                World.DefaultGameObjectInjectionWorld = null;
            ScriptBehaviourUpdateOrder.RemoveWorldFromCurrentPlayerLoop(World);
        }

        protected override void OnUpdate()
        {
            //
        }
    }
}