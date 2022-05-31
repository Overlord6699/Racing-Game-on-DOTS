﻿using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Drift.Ui
{
    [Serializable]
    public class GraphicColor : ComponentTweenBuilder<Graphic>
    {
        [SerializeField]
        private Color endValue;
        
        protected override Tween Build(Graphic component)
        {
            return component.DOColor(endValue, Duration);
        }
    }
}