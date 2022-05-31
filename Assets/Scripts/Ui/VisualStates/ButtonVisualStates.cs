using UnityEngine;
using UnityEngine.UI;

namespace Drift.Ui
{
    [RequireComponent(typeof(VisualStateMachine))]
    public class ButtonVisualStates : Button
    {
        private VisualStateMachine visualStateMachine;

        protected override void Awake()
        {
            visualStateMachine = GetComponent<VisualStateMachine>();
            base.Awake();
        }
        
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            switch (state)
            {
                case SelectionState.Normal:
                    visualStateMachine.GotoState("Normal");
                    break;
                case SelectionState.Highlighted:
                    visualStateMachine.GotoState("Highlighted");
                    break;
                case SelectionState.Pressed:
                    visualStateMachine.GotoState("Pressed");
                    break;
                case SelectionState.Selected:
                    visualStateMachine.GotoState("Highlighted");
                    break;
                case SelectionState.Disabled:
                    visualStateMachine.GotoState("Normal");
                    break;
            }
        }
    }
}