﻿using System.Collections;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Game State: Awaiting player return to corridor state.
    /// Waits for the player to return to the corridor and face the correct direction, ready for the interaction phase of the test to end.
    /// </summary>
    public class State_AwaitingReturnToCorridor : State_Interface
    {
        // The activation radius for the players x, z position for it to be considered "returned".
        private float _radius = 0.4f;

        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Return To Corridor</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Awaiting Return To Corridor"
            });

            stateMachine.StartCoroutine(StateChangeTest(stateMachine));
        }

        private IEnumerator StateChangeTest(GameControllerStateMachine stateMachine)
        {
            Transform playerHeadTransform = PlayerController.cameraTransform;
            
            while (true)
            {
                if (LookDirectionCheck(ref playerHeadTransform) &&
                    BoundsCheck(ref playerHeadTransform))
                    break;
                
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            
            stateMachine.CurrentState = stateMachine.TrialComplete;
        }

        private bool LookDirectionCheck(ref Transform playerHeadTransform)
        {
            Vector3 forward = playerHeadTransform.forward;
            Vector3 flattenedForward = new Vector3(forward.x, 0, forward.z);

            return (Vector3.Dot(flattenedForward, Vector3.right) > 0.866f); // 0.866f : 30 degree angular displacement
        }
        
        private bool BoundsCheck(ref Transform playerHeadTransform)
        {
            Vector3 position = playerHeadTransform.position;
            Vector3 flattenedPosition = new Vector3(position.x, 0, position.z);

            return (Vector3.SqrMagnitude(flattenedPosition) < Mathf.Pow(_radius, 2));
        }

        
        

        
        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, -3.95f, playerPosition.z);

            CameraOffset.position = newPosition;
        }
        
        
        
        
        
        public override void OnDrawGizmos(GameControllerStateMachine stateMachine)
        {
            Gizmos.DrawRay(PlayerController.cameraTransform.position, Vector3.right);
        }
    }
}