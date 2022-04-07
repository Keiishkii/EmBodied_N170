using StateMachine;
using TMPro;
using UnityEngine;

namespace Questionnaire
{
    public class OneQuestionSliderAnswerPanel : MonoBehaviour
    {
        private QuestionnaireCanvas _questionnaireCanvas;
        private GameControllerStateMachine _stateMachine;

        [SerializeField] private TMP_Text _questionOne;



        private void Awake()
        {
            _questionnaireCanvas = FindObjectOfType<QuestionnaireCanvas>();
            _stateMachine = FindObjectOfType<GameControllerStateMachine>();
        }

        public void SetupPanel(BlockQuestion_OneQuestionSliderAnswer question)
        {
            _questionOne.text = question.questionOne;
        }

        public void OnNextButtonPressed()
        {
            //_stateMachine.dataContainer.blockData[_stateMachine.blockIndex].trialData[_stateMachine.trialIndex].AddQuestion
            _questionnaireCanvas.OnQuestionAnswered();
        }
        
    }
}