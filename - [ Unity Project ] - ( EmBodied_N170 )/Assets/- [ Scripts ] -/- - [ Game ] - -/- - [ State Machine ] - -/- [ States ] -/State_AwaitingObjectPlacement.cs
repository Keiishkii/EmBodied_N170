using DataCollection;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_AwaitingObjectPlacement : State_Interface
    {
        public static readonly UnityEvent<bool> ActivateRoomAColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<bool> ActivateRoomBColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<GameControllerStateMachine> ColliderEntered = new UnityEvent<GameControllerStateMachine>();

        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Object Placement</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Awaiting Object Placement",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
            ColliderEntered.AddListener(OnColliderEnter);
            if (stateMachine.CurrentTrialData.activeRoom == Enums.Room.ROOM_A)
                ActivateRoomAColliders.Invoke(true);
            else
                ActivateRoomBColliders.Invoke(true);
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            ColliderEntered.RemoveListener(OnColliderEnter);
            if (stateMachine.CurrentTrialData.activeRoom == Enums.Room.ROOM_A)
                ActivateRoomAColliders.Invoke(false);
            else
                ActivateRoomBColliders.Invoke(false);
        }


        private void OnColliderEnter(GameControllerStateMachine stateMachine)
        {
            stateMachine.SetState(stateMachine.AwaitingReturnToCorridor);
        }
    }
}