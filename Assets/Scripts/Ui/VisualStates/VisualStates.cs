using System;
using System.Linq;
using UnityEngine;

namespace Drift.Ui
{
    [Serializable]
    public class VisualStates
    {
        [SerializeField]
        private VisualState[] states;
        
        public bool TryGetState(string name, out VisualState state)
        {
            state = states.FirstOrDefault(s => s.Name == name);
            return state != null;
        }
    }
}