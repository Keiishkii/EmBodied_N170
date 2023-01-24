using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    /// <summary>
    /// Game State: Questionnaire,
    /// This is an unused state, but it is designed to create a UI in which the player can interact with and write there feelings, thoughts and impressions described via different questions.
    /// </summary>
    public class State_Questionnaire : State_Interface
    {
        // A Unity event to signal the compilation of the questionnaire, used to separate the logic from the state and the questionnaire.
        public static readonly UnityEvent<GameControllerStateMachine> QuestionnaireComplete = new UnityEvent<GameControllerStateMachine>();
        
        // A reference to the main canvas.
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());

        // A reference to the shared hand ray interactor.
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        // A reference to shared hand animation controller.
        private HandAnimationController _handAnimationController;
        private HandAnimationController HandAnimationController => _handAnimationController ?? (_handAnimationController = GameObject.FindObjectOfType<HandAnimationController>());
        
        

        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Questionnaire</color>");
            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = "Questionnaire"
            });
            
            RayInteractionController.RayVisibility = true;
            
            
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, -3.95f, playerPosition.z);

            CameraOffset.position = newPosition;
            
            QuestionnaireComplete.AddListener(OnQuestionnaireCompletion);
            MainCanvas.BeginQuestionnaire();
        }
        
        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            QuestionnaireComplete.RemoveListener(OnQuestionnaireCompletion);
        }

        
        
        private void OnQuestionnaireCompletion(GameControllerStateMachine stateMachine)
        {
            stateMachine.CurrentState = stateMachine.TrialComplete;
        }
    }
}