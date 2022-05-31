using FMOD;
using FMODUnity;
using Unity.Assertions;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Zenject;

namespace Drift.Sounds
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class FMODSoundSystem : SystemBase
    {
        private IFMODSoundService soundService;
        
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            commandBufferSystem = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
        }

        [Inject]
        private void Inject(IFMODSoundService soundService)
        {
            this.soundService = soundService;
        }
        
        protected override void OnUpdate()
        {
            var commands = commandBufferSystem.CreateCommandBuffer();
            Entities.WithNone<FMODSound>().ForEach((Entity entity, Sound sound) =>
            {
                
                var soundDefinition = soundService.GetSoundById(sound.Id);
                var eventDescription = soundService.GetEventDefinition(soundDefinition);
                
                var result = eventDescription.createInstance(out var instance);
                Assert.IsTrue(result == RESULT.OK, "createInstance error: " + result);

                result = instance.start();
                Assert.IsTrue(result == RESULT.OK, "start error: " + result);
                
                if (HasComponent<Volume>(entity))
                {
                    instance.setVolume(GetComponent<Volume>(entity).Value);
                }
                if (HasComponent<Translation>(entity))
                {
                    instance.set3DAttributes(((Vector3) GetComponent<Translation>(entity).Value).To3DAttributes());
                }
                if (HasComponent<OneShot>(entity))
                {
                    instance.release();
                    commands.DestroyEntity(entity);
                    return;
                }
                
                commands.AddComponent(entity, new FMODSound
                {
                    Event = instance
                });

                var parameters = soundDefinition.FloatParameters;
                if (parameters != null && parameters.Length > 0)
                {
                    var floatParameters = commands.AddBuffer<FMODFloatParameter>(entity);
                    floatParameters.Capacity = parameters.Length;
                    floatParameters.Length = parameters.Length;
                    for (var index = 0; index < floatParameters.Length; index++)
                    {
                        floatParameters[index] = new FMODFloatParameter
                        {
                            ParameterId = soundService.GetFloatParameterDescription(
                                soundDefinition, parameters[index]).id
                        };
                    }
                }
                
            }).WithoutBurst().Run();
        }
    }
}