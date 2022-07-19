using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_Questionnaire : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> QuestionnaireComplete = new UnityEvent<GameControllerStateMachine>();
        
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());

        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private HandAnimationController _handAnimationController;
        private HandAnimationController HandAnimationController => _handAnimationController ?? (_handAnimationController = GameObject.FindObjectOfType<HandAnimationController>());
        
        

        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Questionnaire</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Questionnaire"
            });

            HandAnimationController.LeftHandState = HandAnimationState.Default;
            HandAnimationController.RightHandState = HandAnimationState.Default;
            
            RayInteractionController.RayVisibility = true;
            
            
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, -3.95f, playerPosition.z);

            CameraOffset.position = newPosition;
            
            QuestionnaireComplete.AddListener(OnQuestionnaireCompletion);
            MainCanvas.BeginQuestionnaire();
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            QuestionnaireComplete.RemoveListener(OnQuestionnaireCompletion);
        }

        
        
        private void OnQuestionnaireCompletion(GameControllerStateMachine stateMachine)
        {
            stateMachine.SetState(stateMachine.TrialComplete);
        }
    }
}