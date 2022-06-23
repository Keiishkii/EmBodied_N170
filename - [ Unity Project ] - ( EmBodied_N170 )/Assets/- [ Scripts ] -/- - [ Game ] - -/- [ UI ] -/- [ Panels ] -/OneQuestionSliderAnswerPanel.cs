using DataCollection;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
    public class OneQuestionSliderAnswerPanel : QuestionPanel_Interface<BlockQuestion_OneQuestionSliderAnswer>
    {
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private Slider _answerSlider;

        
        
        public override void SetupPanel(ref BlockQuestion_OneQuestionSliderAnswer blockQuestion)
        {
            _questionText.text = blockQuestion.questionOne;
        }

        protected override void WriteResultToDataContainer()
        {
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new SliderQuestionData()
            {
                question = _questionText.text,
                answer = _answerSlider.value
            });
        }
    }
}