﻿using System;
using DG.Tweening;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public class CanvasGroupAlpha : ComponentTweenBuilder<CanvasGroup>
    {
        [SerializeField]
        private float endValue;
        
        protected override Tween Build(CanvasGroup component)
        {
            return component.DOFade(endValue, Duration);
        }
    }
}