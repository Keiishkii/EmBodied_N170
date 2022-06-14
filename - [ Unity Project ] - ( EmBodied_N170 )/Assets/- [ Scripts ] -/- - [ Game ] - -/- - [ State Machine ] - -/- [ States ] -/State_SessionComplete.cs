using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_SessionComplete : State
    {
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Session Complete</color>");
            
            MainCanvas.SessionCompletePanelVisible = true;
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}