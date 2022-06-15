using DataCollection;
using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_BlockStart : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> StartBlock = new UnityEvent<GameControllerStateMachine>();
        
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex;
            Debug.Log($"Entered State: <color=#FFF>Block Start: {blockIndex}</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = $"Block {blockIndex} Start",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
            stateMachine.currentBlock = stateMachine.SessionFormatObject.blocks[blockIndex];
            stateMachine.dataContainer.blockData.Add(new BlockData());

            MainCanvas.AwaitingBlockStartPanelVisible = true;
            
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