using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public abstract class TweenBuilderBase : ITweenBuilder
    {
        [SerializeField]
        private float delay;
        [SerializeField]
        private bool isSpeedBased;
        [SerializeField]
        protected float Duration = 1;
        [SerializeField]
        private Ease ease = Ease.InOutSine;
        [SerializeField]
        private int loopCount = 0;
        [SerializeField]
        private LoopType loopType = LoopType.Yoyo;
        
        public UniTask Build(GameObject gameObject)
        {
            var tween = BuildTween(gameObject);
            
            if (tween == null) return UniTask.CompletedTask;
            
            tween
                .SetDelay(delay)
                .SetSpeedBased(isSpeedBased)
                .SetEase(ease);
            
            if (loopCount != 0)
            {
                tween.SetLoops(loopCount, loopType);
            }
            
            tween.SetTarget(gameObject);
            
            return tween.ToUniTask();
        }
        
        public abstract Tween BuildTween(GameObject gameObject);
    }
}