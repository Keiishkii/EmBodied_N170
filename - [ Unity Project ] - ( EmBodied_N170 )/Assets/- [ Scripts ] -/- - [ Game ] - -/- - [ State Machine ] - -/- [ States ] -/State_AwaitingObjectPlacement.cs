using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    /// <summary>
    /// Game State: Awaiting object placement.
    /// Awaits the players interaction with the NPC and desk, on start enables collisions for the desk.
    /// </summary>
    public class State_AwaitingObjectPlacement : State_Interface
    {
        // Unity events for activating and deactivating room colliders, used to separate the state logic to the scene game objects.
        public static readonly UnityEvent<bool> ActivateRoomAColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<bool> ActivateRoomBColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<GameControllerStateMachine> ObjectPlaced = new UnityEvent<GameControllerStateMachine>();

        // Function for setting the room behaviour on starting and ending the state.
        private delegate void SetRoomColliderActivation(bool activationState);
        private SetRoomColliderActivation _activateRoomCollider;

        
        
        

        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Object Placement</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Awaiting Object Placement"
            });
            
            
            ObjectPlaced.AddListener(OnColliderEnter);
            
            _activateRoomCollider = (stateMachine.currentBlock.targetRoom == Room.RoomA) ? SetRightRoomColliderActivation : SetLeftRoomColliderActivation;
            _activateRoomCollider.Invoke(true);
        }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            ObjectPlaced.RemoveListener(OnColliderEnter);
            
            _activateRoomCollider.Invoke(false);
        }

        
        
        

        private void OnColliderEnter(GameControllerStateMachine stateMachine)
        {
            stateMachine.CurrentState = stateMachine.AwaitingReturnToCorridor;
        }

        private void SetRightRoomColliderActivation(bool activationState)
        {
            ActivateRoomAColliders.Invoke(activationState);
        }
        
        private void SetLeftRoomColliderActivation(bool activationState)
        {
            ActivateRoomBColliders.Invoke(activationState);
        }
    }
}