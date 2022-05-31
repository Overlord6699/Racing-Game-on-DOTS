﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public class LocalRotation : TweenBuilderBase
    {
        [SerializeField]
        private Vector3 endValue;

        [SerializeField] 
        private bool offset;

        public override Tween BuildTween(GameObject gameObject)
        {
            var transform = gameObject.transform;
            return transform.DOLocalRotate(
                offset ? transform.localRotation.eulerAngles + endValue : endValue, 
                Duration);
        }
    }
}