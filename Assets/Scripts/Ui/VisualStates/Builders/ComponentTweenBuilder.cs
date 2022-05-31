using DG.Tweening;
using UnityEngine;

namespace Drift.Ui
{
    public abstract class ComponentTweenBuilder<TComponent> : TweenBuilderBase
    {
        public override Tween BuildTween(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<TComponent>(out var component))
            {
                Debug.LogError($"Component \"{typeof(TComponent)}\" not found on \"{gameObject}\"");
                return null;
            }
            
            return Build(component);
        }
        
        protected abstract Tween Build(TComponent component);
    }
}