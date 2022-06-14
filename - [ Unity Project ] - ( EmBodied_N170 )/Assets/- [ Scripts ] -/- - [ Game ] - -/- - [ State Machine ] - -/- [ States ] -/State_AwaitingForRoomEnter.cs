using UnityEngine;

namespace StateMachine
{
    public class State_AwaitingForRoomEnter : State
    {
        private Transform _playerTransform;
        private Transform PlayerTransform => _playerTransform ?? (_playerTransform = GameObject.FindObjectOfType<Camera>().transform);

        private const float _distanceToRoomEntrance = 1.5f;
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting For Room Enter</color>");
        }

        public override void Update(GameControllerStateMachine stateMachine)
        {
            float playerXCoord = PlayerTransform.position.x;
            if (Mathf.Abs(Vector3.SqrMagnitude(new Vector3(playerXCoord, 0, 0))) > _distanceToRoomEntrance * _distanceToRoomEntrance)
            {
                stateMachine.dataContainer.blockData[stateMachine.blockIndex].trialData[stateMachine.trialIndex].activeRoom = playerXCoord > 0 ? Enums.Room.ROOM_A : Enums.Room.ROOM_B;
                stateMachine.SetState(stateMachine.AwaitingObjectPlacement);
            }
        }
        
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}