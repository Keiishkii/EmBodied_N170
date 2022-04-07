using DataCollection;
using UnityEngine;

namespace StateMachine
{
    public class State_BlockStart : State
    {
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex;
            
            Debug.Log($"Entered State: <color=#FFF>Block Start: {blockIndex}</color>");
            stateMachine.currentBlock = stateMachine.SessionFormatObject.blocks[blockIndex];
            stateMachine.dataContainer.blockData.Add(new BlockData());
            
            if (stateMachine.currentBlock.trials.Count > 0)
            {
                stateMachine.trialIndex = 0;
                stateMachine.SetState(stateMachine.TrialStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.BlockComplete);
            }
        }

        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}