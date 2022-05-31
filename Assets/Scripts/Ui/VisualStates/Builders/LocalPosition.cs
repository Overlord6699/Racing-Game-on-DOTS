﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public class LocalPosition : TweenBuilderBase
    {
        [SerializeField]
        private Vector3 endValue;

        [SerializeField] 
        private bool offset;

        public override Tween BuildTween(GameObject gameObject)
        {
            var transform = gameObject.transform;
            return transform.DOLocalMove(
                offset ? transform.localPosition + endValue : endValue, 
                Duration);
        }
    }
}