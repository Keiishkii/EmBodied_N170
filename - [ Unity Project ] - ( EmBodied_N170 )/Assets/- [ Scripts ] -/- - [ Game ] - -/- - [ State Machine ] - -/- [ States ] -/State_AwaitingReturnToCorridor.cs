using DataCollection;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_AwaitingReturnToCorridor : State_Interface
    {
        public static readonly UnityEvent<Enums.Room> NPCTrigger = new UnityEvent<Enums.Room>();
        
        private const float _distanceTillRoomExited = 1.0f;
        

        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Return To Corridor</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Awaiting Return To Corridor",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
            NPCTrigger.Invoke(stateMachine.CurrentTrialData.activeRoom);
        }

        public override void Update(GameControllerStateMachine stateMachine)
        {
            Vector3 position = CameraTransform.position;
            if (Mathf.Abs(Vector3.SqrMagnitude(new Vector3(position.x, 0, position.z))) < _distanceTillRoomExited * _distanceTillRoomExited)
            {
                stateMachine.SetState(stateMachine.Questionnaire);
            }
        }
        
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
        
    }
}