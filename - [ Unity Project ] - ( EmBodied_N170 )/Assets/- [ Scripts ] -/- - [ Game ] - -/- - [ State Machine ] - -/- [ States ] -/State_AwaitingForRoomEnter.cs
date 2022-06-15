using DataCollection;
using UnityEngine;

namespace StateMachine
{
    public class State_AwaitingForRoomEnter : State_Interface
    {
        private const float _distanceToRoomEntrance = 1.5f;
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting For Room Enter</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Awaiting For Room Enter"
            });
        }

        public override void Update(GameControllerStateMachine stateMachine)
        {
            float playerXCoord = CameraTransform.position.x;
            if (Mathf.Abs(Vector3.SqrMagnitude(new Vector3(playerXCoord, 0, 0))) > _distanceToRoomEntrance * _distanceToRoomEntrance)
            {
                stateMachine.dataContainer.blockData[stateMachine.blockIndex].trialData[stateMachine.trialIndex].activeRoom = playerXCoord > 0 ? Enums.Room.ROOM_A : Enums.Room.ROOM_B;
                stateMachine.SetState(stateMachine.AwaitingObjectPlacement);
            }
        }
        
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}