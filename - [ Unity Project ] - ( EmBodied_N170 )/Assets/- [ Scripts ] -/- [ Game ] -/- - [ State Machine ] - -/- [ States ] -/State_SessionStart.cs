using UnityEngine;

namespace StateMachine
{
    public class State_SessionStart : State
    {
        private Transform _cameraOffset;
        private Transform CameraOffset => _cameraOffset ?? (_cameraOffset = GameObject.FindObjectOfType<PlayerController>().transform);
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Session Start</color>");

            CameraOffset.SetPositionAndRotation(new Vector3(0, -3.95f, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            
            if (stateMachine.SessionFormatObject.blocks.Count > 0)
            {
                stateMachine.blockIndex = 0;
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