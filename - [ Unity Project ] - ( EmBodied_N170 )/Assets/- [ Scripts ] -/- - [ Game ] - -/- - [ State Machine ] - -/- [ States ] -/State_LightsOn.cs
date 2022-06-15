using DataCollection;
using UnityEngine;

namespace StateMachine
{
    public class State_LightsOn : State_Interface
    {
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Lights On</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Lights On",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
            
            CameraOffset.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, 90, 0)));
            
            stateMachine.SetState(stateMachine.AwaitingForRoomEnter);
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}