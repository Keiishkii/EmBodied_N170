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
            
            
            MainCanvas.SessionCompletePanelVisible = true;
            
            DataCollector.WriteData();
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}