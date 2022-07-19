using System.Collections;
using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_BlockComplete : State_Interface
    {
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());

        private RoomCorrectionCanvasController _roomCorrectionCanvasController;
        private RoomCorrectionCanvasController RoomCorrectionCanvasController => _roomCorrectionCanvasController ?? (_roomCorrectionCanvasController = GameObject.FindObjectOfType<RoomCorrectionCanvasController>());
        
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Block Complete</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Block Complete"
            });
            
            
            RoomCorrectionCanvasController.RoomACanvasVisible = false;
            RoomCorrectionCanvasController.RoomBCanvasVisible = false;
            
            MainCanvas.BlockCompletePanelVisible = true;

            stateMachine.StartCoroutine(WaitBeforeExiting(stateMachine));
        }
        
        public override void OnExitState(GameControllerStateMachine stateMachine) 
        {
            MainCanvas.BlockCompletePanelVisible = false;
        }



        private IEnumerator WaitBeforeExiting(GameControllerStateMachine stateMachine)
        {
            stateMachine.blockIndex++;

            yield return new WaitForSeconds(2.0f);
            
            if (stateMachine.SessionFormatObject.blocks.Count > stateMachine.blockIndex)
            {
                stateMachine.SetState(stateMachine.BlockStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.SessionComplete);
            }
        }
    }
}