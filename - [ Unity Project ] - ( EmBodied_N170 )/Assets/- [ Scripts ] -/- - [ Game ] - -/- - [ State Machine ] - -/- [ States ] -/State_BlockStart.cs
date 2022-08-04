using Enums;
using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_BlockStart : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> StartBlock = new UnityEvent<GameControllerStateMachine>();
        
        private RoomCorrectionCanvasController _roomCorrectionCanvasController;
        private RoomCorrectionCanvasController RoomCorrectionCanvasController => _roomCorrectionCanvasController ?? (_roomCorrectionCanvasController = GameObject.FindObjectOfType<RoomCorrectionCanvasController>());
        
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex;
            Debug.Log($"Entered State: <color=#FFF>Block Start: {blockIndex}</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = $"Block {blockIndex} Start"
            });
            
            
            stateMachine.currentBlock = stateMachine.SessionFormatObject.blocks[blockIndex];
            Room activeRoom = stateMachine.currentBlock.targetRoom;
            
            DataCollector.dataContainer.blockData.Add(new Data.DataCollection.BlockData()
            {
                activeRoom = activeRoom
            });
            
            
            MainCanvas.AwaitingBlockStartPanelVisible = true;
            MainCanvas.blockDescriptionLabel.text = 
                $"This block uses the <color=#55FFAA>{((activeRoom == Room.RoomA) ? ("approach") : ("withdraw"))}</color> condition";

            if (activeRoom == Room.RoomA)
            {
                RoomCorrectionCanvasController.RoomACanvasVisible = false;
                RoomCorrectionCanvasController.RoomBCanvasVisible = true;
            }
            else
            {
                RoomCorrectionCanvasController.RoomACanvasVisible = true;
                RoomCorrectionCanvasController.RoomBCanvasVisible = false;
            }
            
            StartBlock.AddListener(OnBlockStart);
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            MainCanvas.AwaitingBlockStartPanelVisible = false;
            
            StartBlock.RemoveListener(OnBlockStart);
        }

        private void OnBlockStart(GameControllerStateMachine stateMachine)
        {
            if (stateMachine.currentBlock.trials.Count > 0)
            {
                stateMachine.trialIndex = 0;
                stateMachine.SetState(stateMachine.TrialStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.BlockComplete);
            }   
        }
    }
}