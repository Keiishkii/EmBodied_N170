using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Game State: Completion of a trial.
    /// Cleans the scene of the objects of the previous trial and gets ready to move on the the next trial, or block.
    /// </summary>
    public class State_TrialComplete : State_Interface
    {
        // Reference to the scene NPC Manager
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
    }
}