using Enums;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Game State: Awaiting room entrance state.
    /// Waits for the player to enter the correct room, before continuing to the next state.
    /// </summary>
    public class State_AwaitingForRoomEnter : State_Interface
    {
        // Displacement along the x axis until the player has entered the correct room.
        private const float _distanceToRoomEntrance = 1.5f;
        
        // Comparison function for room distance checks.
        private delegate bool ComparisonCheck(in Vector3 position);
        private ComparisonCheck _comparisonCheck;
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting For Room Enter</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Awaiting For Room Enter"
            });

            
            _comparisonCheck = (stateMachine.currentBlock.targetRoom == Room.RoomA) ? RightRoomDistanceCheck : LeftRoomDistanceCheck;
        }

        public override void Update(GameControllerStateMachine stateMachine)
        {
            if (_comparisonCheck(CameraTransform.position))
            {
                stateMachine.CurrentState = stateMachine.AwaitingObjectPlacement;
            }
        }



        private static bool RightRoomDistanceCheck(in Vector3 position)
        {
            return (position.x > _distanceToRoomEntrance);
        }
        
        private static bool LeftRoomDistanceCheck(in Vector3 position)
        {
            return (position.x < -_distanceToRoomEntrance);
        }
    }
}