using System.Collections;
using DataCollection;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_AwaitingReturnToCorridor : State_Interface
    {
        private const float _distanceTillRoomExited = 1.0f;
        private const float _dotResultComparison = 0.995f;
        

        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Awaiting Return To Corridor</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Awaiting Return To Corridor"
            });

            stateMachine.StartCoroutine(StateChangeTest(stateMachine));
        }

        private IEnumerator StateChangeTest(GameControllerStateMachine stateMachine)
        {
            while (true)
            {
                Vector3 position = CameraTransform.position;
                if (Mathf.Abs(Vector3.SqrMagnitude(new Vector3(position.x, 0, position.z))) < Mathf.Pow(_distanceTillRoomExited, 2))
                {
                    Vector3 flattenedForward = Vector3.Normalize(new Vector3(CameraTransform.forward.x, 0, CameraTransform.forward.z));
                    float result = Vector3.Dot(
                        flattenedForward,
                        Vector3.right);

                    if (result > 0.995f)
                    {
                        break;
                    }
                }
                
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            
            stateMachine.SetState(stateMachine.Questionnaire);
        }
    }
}