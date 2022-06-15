using DataCollection;
using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_SessionStart : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> StartSession = new UnityEvent<GameControllerStateMachine>();
        
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());





        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Session Start</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Session Start",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
            CameraOffset.SetPositionAndRotation(new Vector3(0, -3.95f, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            MainCanvas.AwaitingSessionStartPanelVisible = true;
            
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