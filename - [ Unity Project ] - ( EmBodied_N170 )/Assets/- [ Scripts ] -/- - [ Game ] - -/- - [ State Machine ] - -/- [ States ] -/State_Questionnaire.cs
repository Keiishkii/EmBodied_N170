using Questionnaire;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    public class State_Questionnaire : State
    {
        public static readonly UnityEvent QuestionnaireComplete = new UnityEvent();
        
        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());
        
        private GameControllerStateMachine _stateMachine;
        
        private Transform _cameraOffset;
        private Transform CameraOffset => _cameraOffset ?? (_cameraOffset = GameObject.FindObjectOfType<PlayerController>().transform);
        
        
        
        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#FFF>Questionnaire</color>");
            _stateMachine = stateMachine;
            
            Vector3 playerPosition = CameraOffset.position;
            Vector3 newPosition = new Vector3(playerPosition.x, -3.95f, playerPosition.z);
            
            CameraOffset.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
            
            QuestionnaireComplete.AddListener(OnQuestionnaireCompletion);
            MainCanvas.BeginQuestionnaire();
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