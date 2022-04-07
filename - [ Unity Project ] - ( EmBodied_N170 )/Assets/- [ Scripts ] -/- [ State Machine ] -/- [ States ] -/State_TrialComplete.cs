using UnityEngine;

namespace StateMachine
{
    public class State_TrialComplete : State
    {
        private PlayerController _playerController;
        private PlayerController PlayerController => _playerController ?? (_playerController = GameObject.FindObjectOfType<PlayerController>());
        
        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Trial Complete</color>");
            
            PlayerController.DestroyHeldItem();
            NPCManager.DestroyNPC();
            
            stateMachine.trialIndex++;
            if (stateMachine.currentBlock.trials.Count > stateMachine.trialIndex)
            {
                stateMachine.SetState(stateMachine.TrialStart);
            }
            else
            {
                stateMachine.SetState(stateMachine.BlockComplete);
            }
        }

        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }
    }
}