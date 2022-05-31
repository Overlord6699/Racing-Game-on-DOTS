using System;
using System.Reflection;

namespace Drift.Ui
{
    public static class ReflectionExtensions
    {
        public static PropertyOrField GetPublicFieldOrProperty(this Type containerType, string name)
        {
            var field = containerType.GetField(name, BindingFlags.Instance | BindingFlags.Public);
            if (field != null) return new PropertyOrField(field);
            var property = containerType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
            if (property != null) return new PropertyOrField(property);
            
            throw new Exception($"Public Field or Property \"{name}\" not found in type \"{containerType}\"");
        }

        public static T GetPublicFieldOrPropertyValue<T>(this object container, string name)
        {
            var containerType = container.GetType();
            var propertyOrField = containerType.GetPublicFieldOrProperty(name);
            
            var expectedPropertyType = typeof(T);
            if (!propertyOrField.IsOfType(expectedPropertyType))
            {
                throw new Exception($"Public field or property \"{name}\" in type \"{containerType}\" " +
                                    $"has wrong type \"{propertyOrField.Type}\", " +
                                    $"but expected \"{expectedPropertyType}\"");
            }
            
            return propertyOrField.GetValue<T>(container);
        }
    }
}