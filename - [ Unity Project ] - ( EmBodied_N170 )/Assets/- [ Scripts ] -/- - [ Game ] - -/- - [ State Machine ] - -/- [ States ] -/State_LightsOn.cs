using System.Collections;
using UnityEngine;

namespace StateMachine
{
    public class State_LightsOn : State_Interface
    {
        /// <summary>
        /// Game State: Lights on State.
        /// Marks the beginning of the player interaction phase for the trial. Waits a second before signalling to the player to begin movement.
        /// </summary>
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
            yield return new WaitForSeconds(1.0f);
            GlobalAudioSource.Instance.PlayOneShot(GlobalAudioSource.Instance.SfxContainer.GoSfx);
            
            stateMachine.CurrentState = stateMachine.AwaitingForRoomEnter;
        }
    }
}