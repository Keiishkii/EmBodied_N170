using DataCollection;
using Questionnaire;
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
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = "Trial Complete",
                
                SetHeadTransform = CameraTransform,
                SetLeftHandTransform = LeftHandTransform,
                SetRightHandTransform = RightHandTransform
            });
            
            
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