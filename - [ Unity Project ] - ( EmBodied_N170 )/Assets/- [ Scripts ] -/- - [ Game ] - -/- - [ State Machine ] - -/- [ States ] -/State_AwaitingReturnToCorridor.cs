using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public class State_AwaitingReturnToCorridor : State_Interface
    {
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
            
            stateMachine.SetState(stateMachine.Questionnaire);
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

        
        
        
        
        public override void OnDrawGizmos(GameControllerStateMachine stateMachine)
        {
            Gizmos.DrawRay(PlayerController.cameraTransform.position, Vector3.right);
        }
    }
}