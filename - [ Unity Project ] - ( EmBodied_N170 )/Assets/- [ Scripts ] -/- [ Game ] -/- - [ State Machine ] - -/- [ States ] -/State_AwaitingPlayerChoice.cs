using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_AwaitingPlayerChoice : State
    {
        public static readonly UnityEvent<bool> ActivateRoomAColliders = new UnityEvent<bool>();
        public static readonly UnityEvent<bool> ActivateRoomBColliders = new UnityEvent<bool>();
        public static readonly UnityEvent ColliderEntered = new UnityEvent();

        private GameControllerStateMachine _stateMachine;
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Player Choice</color>");
            _stateMachine = stateMachine;
            
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


        private void OnColliderEnter()
        {
            _stateMachine.SetState(_stateMachine.AwaitingReturnToCorridor);
        }
    }
}