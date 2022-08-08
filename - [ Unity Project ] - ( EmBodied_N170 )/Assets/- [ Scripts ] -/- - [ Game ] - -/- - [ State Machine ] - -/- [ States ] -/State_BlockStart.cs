using Enums;
using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_BlockStart : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> StartBlock = new UnityEvent<GameControllerStateMachine>();
        public static readonly UnityEvent<GameControllerStateMachine> ExitEarly = new UnityEvent<GameControllerStateMachine>();
        
        private RoomCorrectionCanvasController _roomCorrectionCanvasController;
        private RoomCorrectionCanvasController RoomCorrectionCanvasController => _roomCorrectionCanvasController ?? (_roomCorrectionCanvasController = GameObject.FindObjectOfType<RoomCorrectionCanvasController>());
        
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        private StopSessionEarlyCanvas _stopSessionEarlyCanvas;
        private StopSessionEarlyCanvas StopSessionEarlyCanvas => _stopSessionEarlyCanvas ?? (_stopSessionEarlyCanvas = GameObject.FindObjectOfType<StopSessionEarlyCanvas>());
        
        
        
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
            
            RayInteractionController.RayVisibility = true;
            
            StopSessionEarlyCanvas.SetVisible = true;
            
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
            ExitEarly.AddListener(OnExitEarly);
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            MainCanvas.AwaitingBlockStartPanelVisible = false;
            StopSessionEarlyCanvas.SetVisible = false;
            
            StartBlock.RemoveListener(OnBlockStart);
            ExitEarly.RemoveListener(OnExitEarly);
        }

        private void OnBlockStart(GameControllerStateMachine stateMachine)
        {
            if (stateMachine.currentBlock.trials.Count > 0)
            {
                stateMachine.trialIndex = 0;
                stateMachine.CurrentState = stateMachine.TrialStart;
            }
            else
            {
                stateMachine.CurrentState = stateMachine.BlockComplete;
            }
        }

        private void OnExitEarly(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Stopping Session Early");
            stateMachine.CurrentState = stateMachine.SessionComplete;
        }
    }
}