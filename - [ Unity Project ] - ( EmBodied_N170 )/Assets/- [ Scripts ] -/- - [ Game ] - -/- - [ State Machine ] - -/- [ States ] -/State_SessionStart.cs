using System;
using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_SessionStart : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> StartSession = new UnityEvent<GameControllerStateMachine>();
        
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());
        
        private HandAnimationController _handAnimationController;
        private HandAnimationController HandAnimationController => _handAnimationController ?? (_handAnimationController = GameObject.FindObjectOfType<HandAnimationController>());
        
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());





        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Session Start</color>");
            DataCollector.BeginTransformDataCollection();
            DataCollector.dataContainer.dateTime = $"{DateTime.Now:U}";
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Session Start"
            });
                        
            RayInteractionController.RayVisibility = true;
            MainCanvas.AwaitingSessionStartPanelVisible = true;
            
            Vector3 newPosition = new Vector3(0, -3.95f, 0);
            CameraOffset.position = newPosition;
            
            StartSession.AddListener(OnSessionStart);
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            MainCanvas.AwaitingSessionStartPanelVisible = false;
            
            StartSession.RemoveListener(OnSessionStart);
        }

        private void OnSessionStart(GameControllerStateMachine stateMachine)
        {
            if (stateMachine.SessionFormatObject.blocks.Count > 0)
            {
                stateMachine.blockIndex = 0;
                stateMachine.SetState(stateMachine.BlockStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.SessionComplete);
            }
        }
    }
}