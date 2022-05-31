using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public class VisualState
    {
        [SerializeField]
        private string name;

        [SerializeReference]
        [SR]
        private ITweenBuilder[] builders;

        public string Name => name;
        
        public IEnumerable<UniTask> BuildSequence(Component target)
        {
            var gameObject = target.gameObject;
            foreach (var builder in builders)
            {
                yield return builder.Build(gameObject);
            }
        }
    }
}