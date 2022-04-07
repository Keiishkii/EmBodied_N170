using UnityEngine;

namespace StateMachine
{
    public class State_AwaitingReturnToCorridor : State
    {
        private Transform _playerTransform;
        private Transform PlayerTransform => _playerTransform ?? (_playerTransform = GameObject.FindObjectOfType<Camera>().transform);

        private const float _distanceTillRoomExited = 1.0f;
        

        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Return To Corridor</color>");
        }

        public override void Update(GameControllerStateMachine stateMachine)
        {
            Vector3 position = PlayerTransform.position;
            if (Mathf.Abs(Vector3.SqrMagnitude(new Vector3(position.x, 0, position.z))) < _distanceTillRoomExited * _distanceTillRoomExited)
            {
                stateMachine.SetState(stateMachine.Questionnaire);
            }
        }
        
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
        
    }
}