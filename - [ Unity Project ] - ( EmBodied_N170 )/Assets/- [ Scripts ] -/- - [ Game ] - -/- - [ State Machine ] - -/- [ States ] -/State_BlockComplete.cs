using System.Collections;
using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_BlockComplete : State
    {
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Block Complete</color>");
            
            MainCanvas.BlockCompletePanelVisible = true;

            stateMachine.StartCoroutine(WaitBeforeExiting(stateMachine));
        }

        public override void Update(GameControllerStateMachine stateMachine) { }
        
        public override void OnExitState(GameControllerStateMachine stateMachine) 
        {
            MainCanvas.BlockCompletePanelVisible = false;
        }



        private IEnumerator WaitBeforeExiting(GameControllerStateMachine stateMachine)
        {
            stateMachine.blockIndex++;

            yield return new WaitForSeconds(2.0f);
            
            if (stateMachine.SessionFormatObject.blocks.Count > stateMachine.blockIndex)
            {
                stateMachine.SetState(stateMachine.BlockStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.SessionComplete);
            }
        }
    }
}