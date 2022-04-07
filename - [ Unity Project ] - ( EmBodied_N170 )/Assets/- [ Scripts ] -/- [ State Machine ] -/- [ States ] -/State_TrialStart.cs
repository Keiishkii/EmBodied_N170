using System.Collections;
using DataCollection;
using UnityEngine;

namespace StateMachine
{
    public class State_TrialStart : State
    {
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private PlayerController _playerController;
        private PlayerController PlayerController => _playerController ?? (_playerController = GameObject.FindObjectOfType<PlayerController>());
        
        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex;
            int trialIndex = stateMachine.trialIndex;
            
            Debug.Log($"Entered State: <color=#FFF>Trial Start: {trialIndex}</color>");
            stateMachine.currentTrial = stateMachine.currentBlock.trials[trialIndex];
            stateMachine.dataContainer.blockData[blockIndex].trialData.Add(new TrialData());

            RayInteractionController.SetLaserVisibility(false);
            PlayerController.CreateHeldItem(stateMachine.currentTrial.heldObject);
            NPCManager.CreateNPCs(stateMachine.currentTrial.NPCAvatar);

            stateMachine.StartCoroutine(LightsOnState(stateMachine, Random.Range(2.0f, 3.5f)));
        }

        public override void Update(GameControllerStateMachine stateMachine) { }
        public override void OnExitState(GameControllerStateMachine stateMachine) { }


        private IEnumerator LightsOnState(GameControllerStateMachine stateMachine, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            stateMachine.SetState(stateMachine.LightsOn);
        }
    }
}