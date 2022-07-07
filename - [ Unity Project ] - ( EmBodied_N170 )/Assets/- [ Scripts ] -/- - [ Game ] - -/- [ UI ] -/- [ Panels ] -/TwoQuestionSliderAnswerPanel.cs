using System;
using System.Collections;
using System.Collections.Generic;
using DataCollection;
using Enums;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
    public class TwoQuestionSliderAnswerPanel : QuestionPanel_Interface<BlockQuestion_TwoQuestionSliderAnswer>
    {
        [SerializeField] private TMP_Text _questionOneText;
        [SerializeField] private Slider _questionOneSlider;
        
        [Space(5)]
        [SerializeField] private TMP_Text _questionOneMinText;
        [SerializeField] private TMP_Text _questionOneMaxText;
        [SerializeField] private Image _questionOneMinSprite;
        [SerializeField] private Image _questionOneMaxSprite;
        
        [Space(10)]
        [SerializeField] private TMP_Text _questionTwoText;
        [SerializeField] private Slider _questionTwoSlider;
        
        [Space(5)]
        [SerializeField] private TMP_Text _questionTwoMinText;
        [SerializeField] private TMP_Text _questionTwoMaxText;
        [SerializeField] private Image _questionTwoMinSprite;
        [SerializeField] private Image _questionTwoMaxSprite;
        
        
        
        public override void SetupPanel(ref BlockQuestion_TwoQuestionSliderAnswer blockQuestion)
        {
            _questionOneText.text = blockQuestion.sliderOneQuestion;
            switch (blockQuestion.sliderOneQuestionDecorType)
            {
                case QuestionDecor_Enum.Text:
                {
                    _questionOneMinText.gameObject.SetActive(true);
                    _questionOneMaxText.gameObject.SetActive(true);
                    _questionOneMinSprite.gameObject.SetActive(false);
                    _questionOneMaxSprite.gameObject.SetActive(false);
                    
                    _questionOneMinText.text = blockQuestion.sliderOneQuestionDecorMinimumText;
                    _questionOneMaxText.text = blockQuestion.sliderOneQuestionDecorMaximumText;
                    
                } break;
                case QuestionDecor_Enum.Image:
                {
                    _questionOneMinText.gameObject.SetActive(false);
                    _questionOneMaxText.gameObject.SetActive(false);
                    _questionOneMinSprite.gameObject.SetActive(true);
                    _questionOneMaxSprite.gameObject.SetActive(true);

                    Texture2D minTexture = blockQuestion.sliderOneQuestionDecorMinimumSprite, maxTexture = blockQuestion.sliderOneQuestionDecorMaximumSprite;
                    if (minTexture != null) _questionOneMinSprite.overrideSprite = Sprite.Create(minTexture, new Rect(0, 0, minTexture.width, minTexture.height), new Vector2(0.5f, 0.5f));
                    if (maxTexture != null) _questionOneMaxSprite.overrideSprite = Sprite.Create(maxTexture, new Rect(0, 0, maxTexture.width, maxTexture.height), new Vector2(0.5f, 0.5f));
                } break;
            }
            
            _questionTwoText.text = blockQuestion.sliderTwoQuestion;
            switch (blockQuestion.sliderTwoQuestionDecorType)
            {
                case QuestionDecor_Enum.Text:
                {
                    _questionTwoMinText.gameObject.SetActive(true);
                    _questionTwoMaxText.gameObject.SetActive(true);
                    _questionTwoMinSprite.gameObject.SetActive(false);
                    _questionTwoMaxSprite.gameObject.SetActive(false);
                    
                    _questionTwoMinText.text = blockQuestion.sliderTwoQuestionDecorMinimumText;
                    _questionTwoMaxText.text = blockQuestion.sliderTwoQuestionDecorMaximumText;
                    
                } break;
                case QuestionDecor_Enum.Image:
                {
                    _questionTwoMinText.gameObject.SetActive(false);
                    _questionTwoMaxText.gameObject.SetActive(false);
                    _questionTwoMinSprite.gameObject.SetActive(true);
                    _questionTwoMaxSprite.gameObject.SetActive(true);

                    Texture2D minTexture = blockQuestion.sliderTwoQuestionDecorMinimumSprite, maxTexture = blockQuestion.sliderTwoQuestionDecorMaximumSprite;
                    if (minTexture != null) _questionTwoMinSprite.overrideSprite = Sprite.Create(minTexture, new Rect(0, 0, minTexture.width, minTexture.height), new Vector2(0.5f, 0.5f));
                    if (maxTexture != null) _questionTwoMaxSprite.overrideSprite = Sprite.Create(maxTexture, new Rect(0, 0, maxTexture.width, maxTexture.height), new Vector2(0.5f, 0.5f));
                } break;
            }
        }

        protected override void WriteResultToDataContainer()
        {
            Debug.Log("<color=#2CCED3> Writing Question Data </color>");
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new SliderQuestionData()
            {
                question = _questionOneText.text,
                answer = _questionOneSlider.value
            });
            
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new SliderQuestionData()
            {
                question = _questionTwoText.text,
                answer = _questionTwoSlider.value
            });
        }
    }
}