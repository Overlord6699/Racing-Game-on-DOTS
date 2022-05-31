using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Drift.Ui
{
    public static class BindingExtensions
    {
        public static IValue<T> GetProperty<T>(this object container, string name)
        {
            var containerType = container.GetType();
            var propertyOrField = containerType.GetPublicFieldOrProperty(name);
            
            if (propertyOrField.IsOfType(typeof(IValue<T>)))
                return propertyOrField.GetValue<IValue<T>>(container);
            
            throw new Exception($"Public field or property \"{name}\" in type \"{containerType}\" " +
                                $"has wrong type \"{propertyOrField.Type}\", " +
                                $"but expected \"{typeof(IValue<T>)}\"");
        }
        
        public static IValue GetProperty(this object container, string name)
        {
            var containerType = container.GetType();
            var propertyOrField = containerType.GetPublicFieldOrProperty(name);
            if (propertyOrField.IsOfType(typeof(IValue)))
                return propertyOrField.GetValue<IValue>(container);
            throw new Exception($"Public field or property \"{name}\" in type \"{containerType}\" " +
                                $"has wrong type \"{propertyOrField.Type}\", " +
                                $"but expected \"{typeof(IValue)}\"");
        }

        public static void SetDataContextRecursive(this Component source, object dataContext)
        {
            SetDataContextRecursive(source, source.transform, dataContext);
        }

        private static void SetDataContextRecursive(Component source, Transform transform, object dataContext)
        {
            var tempList = ListPool<IBinding>.Get();
            transform.GetComponents(tempList);
            var stopForwarding = false;

            foreach (var binding in tempList)
            {
                if (ReferenceEquals(source, binding)) continue;

                if ((binding.ForwardedBindingStrategy & ForwardedBindingStrategy.Accept) > 0)
                {
                    try
                    {
                        binding.DataContext = dataContext;
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                
                if ((binding.ForwardedBindingStrategy & ForwardedBindingStrategy.Forward) == 0)
                {
                    stopForwarding = true;
                    break;
                }
            }
            
            if (!stopForwarding)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    SetDataContextRecursive(child, dataContext);
                }
            }

            ListPool<IBinding>.Release(tempList);
        }
    }
}