using UnityEngine;

namespace StateMachine
{
    public class State_TrialComplete : State_Interface
    {
        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Trial Complete</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Trial Complete"
            });
            
            
            PlayerController.DestroyHeldItem();
            NPCManager.DestroyNPC();
            
            stateMachine.trialIndex++;
            if (stateMachine.currentBlock.trials.Count > stateMachine.trialIndex)
            {
                stateMachine.CurrentState = stateMachine.TrialStart;
            }
            else
            {
                stateMachine.CurrentState = stateMachine.BlockComplete;
            }
        }

        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}