using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_Questionnaire : State
    {
        public static readonly UnityEvent QuestionnaireComplete = new UnityEvent();
        
        private QuestionnaireCanvas _questionnaireCanvas;
        private QuestionnaireCanvas QuestionnaireCanvas => _questionnaireCanvas ?? (_questionnaireCanvas = GameObject.FindObjectOfType<QuestionnaireCanvas>());
        
        private GameControllerStateMachine _stateMachine;
        
        private Transform _cameraOffset;
        private Transform CameraOffset => _cameraOffset ?? (_cameraOffset = GameObject.FindObjectOfType<PlayerController>().transform);
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Questionnaire</color>");
            _stateMachine = stateMachine;
            
            CameraOffset.SetPositionAndRotation(new Vector3(0, -3.95f, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            
            QuestionnaireComplete.AddListener(OnQuestionnaireCompletion);
            QuestionnaireCanvas.BeginQuestionnaire(ref stateMachine);
        }
        
        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            QuestionnaireComplete.RemoveListener(OnQuestionnaireCompletion);
        }

        
        private void OnQuestionnaireCompletion()
        {
            _stateMachine.SetState(_stateMachine.TrialComplete);
        }
    }
}