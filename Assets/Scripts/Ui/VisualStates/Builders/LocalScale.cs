﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public class LocalScale : TweenBuilderBase
    {
        [SerializeField]
        private Vector3 endValue;

        [SerializeField] 
        private bool relative;

        public override Tween BuildTween(GameObject gameObject)
        {
            var transform = gameObject.transform;
            return transform.DOScale(
                relative ? Vector3.Scale(transform.localScale, endValue) : endValue, 
                Duration);
        }
    }
}