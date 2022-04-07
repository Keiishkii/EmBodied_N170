using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_AwaitingPlayerChoice : State
    {
        public static readonly UnityEvent<bool> ActivateRoomAColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<bool> ActivateRoomBColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<Enums.PlacementChoice> ColliderEntered = new UnityEvent<Enums.PlacementChoice>();

        private GameControllerStateMachine _stateMachine;
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Player Choice</color>");
            _stateMachine = stateMachine;
            
            ColliderEntered.AddListener(OnColliderEnter);
            if (stateMachine.dataContainer.blockData[stateMachine.blockIndex].trialData[stateMachine.trialIndex].activeRoom == Enums.Room.ROOM_A)
                ActivateRoomAColliders.Invoke(true);
            else
                ActivateRoomBColliders.Invoke(true);
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            ColliderEntered.RemoveListener(OnColliderEnter);
            if (stateMachine.dataContainer.blockData[stateMachine.blockIndex].trialData[stateMachine.trialIndex].activeRoom == Enums.Room.ROOM_A)
                ActivateRoomAColliders.Invoke(false);
            else
                ActivateRoomBColliders.Invoke(false);
        }


        private void OnColliderEnter(Enums.PlacementChoice placementChoice)
        {
            _stateMachine.dataContainer.blockData[_stateMachine.blockIndex].trialData[_stateMachine.trialIndex].placementChoice = placementChoice;
            
            _stateMachine.SetState(_stateMachine.AwaitingReturnToCorridor);
        }
    }
}