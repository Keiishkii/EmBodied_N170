using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public class State_LightsOn : State_Interface
    {
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Lights On</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Lights On"
            });
            
            
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
            
            CameraOffset.position = newPosition;

            stateMachine.StartCoroutine(LightsOnCountDown(stateMachine));
        }

        private IEnumerator LightsOnCountDown(GameControllerStateMachine stateMachine)
        {
            yield return new WaitForSeconds(0.1f);
            GlobalAudioSource.Instance.PlayOneShot(GlobalAudioSource.Instance.SfxContainer.GoSfx);
            
            stateMachine.CurrentState = stateMachine.AwaitingForRoomEnter;
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}