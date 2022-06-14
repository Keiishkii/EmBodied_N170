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

            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
            
            CameraOffset.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, 90, 0)));
            
            stateMachine.SetState(stateMachine.AwaitingForRoomEnter);
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}