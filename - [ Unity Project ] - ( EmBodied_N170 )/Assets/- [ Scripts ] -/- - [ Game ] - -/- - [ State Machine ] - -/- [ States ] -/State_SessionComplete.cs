using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_SessionComplete : State_Interface
    {
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Session Complete</color>");
            DataCollector.EndTransformDataCollection();
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Session Complete"
            });
            
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, -3.95f, playerPosition.z);

            CameraOffset.position = newPosition;
            
            MainCanvas.SessionCompletePanelVisible = true;
            
            DataCollector.WriteData();
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}