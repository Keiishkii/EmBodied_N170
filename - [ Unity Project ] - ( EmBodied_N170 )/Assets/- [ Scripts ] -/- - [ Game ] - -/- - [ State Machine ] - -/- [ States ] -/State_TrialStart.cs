using System.Collections;
using DataCollection;
using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_TrialStart : State_Interface
    {
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());

        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex, trialIndex = stateMachine.trialIndex;
            
            Debug.Log($"Entered State: <color=#FFF>Trial Start: {trialIndex}</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = $"Trial {trialIndex} Start",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
            MainCanvas.ReadyPanelVisible = true;
            
            stateMachine.currentTrial = stateMachine.currentBlock.trials[trialIndex];
            Data.Trial currentTrial = stateMachine.currentTrial;
            stateMachine.dataContainer.blockData[blockIndex].trialData.Add(new TrialData());

            RayInteractionController.RayVisibility = false;
            PlayerController.CreateHeldItem(currentTrial.heldObject);
            NPCManager.CreateNPCs(currentTrial.roomA_NPCAvatar, currentTrial.roomB_NPCAvatar);

            stateMachine.StartCoroutine(LightsOnState(stateMachine, Random.Range(1.0f, 1.5f)));
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            MainCanvas.ReadyPanelVisible = false;
        }


        private IEnumerator LightsOnState(GameControllerStateMachine stateMachine, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            
            stateMachine.SetState(stateMachine.LightsOn);
        }
    }
}