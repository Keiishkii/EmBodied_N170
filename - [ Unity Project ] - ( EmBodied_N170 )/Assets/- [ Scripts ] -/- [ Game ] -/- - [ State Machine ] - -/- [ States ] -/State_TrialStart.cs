using System.Collections;
using DataCollection;
using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_TrialStart : State
    {
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private PlayerController _playerController;
        private PlayerController PlayerController => _playerController ?? (_playerController = GameObject.FindObjectOfType<PlayerController>());
        
        private QuestionnaireCanvas _questionnaireCanvas;
        private QuestionnaireCanvas QuestionnaireCanvas => _questionnaireCanvas ?? (_questionnaireCanvas = GameObject.FindObjectOfType<QuestionnaireCanvas>());

        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex;
            int trialIndex = stateMachine.trialIndex;
            
            Debug.Log($"Entered State: <color=#FFF>Trial Start: {trialIndex}</color>");
            
            QuestionnaireCanvas.ShowReadyPanel(true);
            
            stateMachine.currentTrial = stateMachine.currentBlock.trials[trialIndex];
            Data.Trial currentTrial = stateMachine.currentTrial;
            stateMachine.dataContainer.blockData[blockIndex].trialData.Add(new TrialData());

            RayInteractionController.SetLaserVisibility(false);
            PlayerController.CreateHeldItem(currentTrial.heldObject);
            NPCManager.CreateNPCs(currentTrial.roomA_NPCAvatar, currentTrial.roomB_NPCAvatar);

            stateMachine.StartCoroutine(LightsOnState(stateMachine, Random.Range(1.0f, 1.5f)));
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            QuestionnaireCanvas.ShowReadyPanel(false);
        }


        private IEnumerator LightsOnState(GameControllerStateMachine stateMachine, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            stateMachine.SetState(stateMachine.LightsOn);
        }
    }
}