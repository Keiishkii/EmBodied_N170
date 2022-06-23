using System;
using System.Collections;
using System.Collections.Generic;
using DataCollection;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
    public class TwoQuestionSliderAnswerPanel : QuestionPanel_Interface<BlockQuestion_TwoQuestionSliderAnswer>
    {
        [SerializeField] private TMP_Text _questionOneText;
        [SerializeField] private Slider _answerTwoSlider;
        
        [Space(5)]
        [SerializeField] private TMP_Text _questionTwoText;
        [SerializeField] private Slider _answerOneSlider;
        
        
        
        public override void SetupPanel(ref BlockQuestion_TwoQuestionSliderAnswer blockQuestion)
        {
            _questionOneText.text = blockQuestion.questionOne;
            _questionTwoText.text = blockQuestion.questionTwo;
        }

        protected override void WriteResultToDataContainer()
        {
            Debug.Log("<color=#FF0000> Writing Question Data </color>");
            
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new SliderQuestionData()
            {
                question = _questionOneText.text,
                answer = _answerOneSlider.value
            });
            
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new SliderQuestionData()
            {
                question = _questionTwoText.text,
                answer = _answerTwoSlider.value
            });
        }
    }
}