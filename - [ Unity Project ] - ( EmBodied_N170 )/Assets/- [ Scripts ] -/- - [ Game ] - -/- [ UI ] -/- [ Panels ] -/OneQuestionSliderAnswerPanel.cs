using StateMachine;
using TMPro;
using UnityEngine;

namespace Questionnaire
{
    public class OneQuestionSliderAnswerPanel : MonoBehaviour
    {
        private MainCanvas _mainCanvas;
        private GameControllerStateMachine _stateMachine;

        [SerializeField] private TMP_Text _questionOne;



        private void Awake()
        {
            _mainCanvas = FindObjectOfType<MainCanvas>();
            _stateMachine = FindObjectOfType<GameControllerStateMachine>();
        }

        public void SetupPanel(BlockQuestion_OneQuestionSliderAnswer question)
        {
            _questionOne.text = question.questionOne;
        }

        public void OnNextButtonPressed()
        {
            //_stateMachine.dataContainer.blockData[_stateMachine.blockIndex].trialData[_stateMachine.trialIndex].AddQuestion
            _mainCanvas.OnQuestionAnswered();
        }
        
    }
}