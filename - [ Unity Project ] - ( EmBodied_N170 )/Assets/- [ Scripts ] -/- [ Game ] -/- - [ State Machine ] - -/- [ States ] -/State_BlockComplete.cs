using UnityEngine;

namespace StateMachine
{
    public class State_BlockComplete : State
    {
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Block Complete</color>");
            
            stateMachine.blockIndex++;
            if (stateMachine.SessionFormatObject.blocks.Count > stateMachine.blockIndex)
            {
                stateMachine.SetState(stateMachine.BlockStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.SessionComplete);
            }
        }

        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}