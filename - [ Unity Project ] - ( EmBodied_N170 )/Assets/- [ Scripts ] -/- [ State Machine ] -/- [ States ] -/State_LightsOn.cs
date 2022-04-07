using UnityEngine;

namespace StateMachine
{
    public class State_LightsOn : State
    {
        private Transform _cameraOffset;
        private Transform CameraOffset => _cameraOffset ?? (_cameraOffset = GameObject.FindObjectOfType<PlayerController>().transform);
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Lights On</color>");
            CameraOffset.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            stateMachine.SetState(stateMachine.AwaitingForRoomEnter);
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}