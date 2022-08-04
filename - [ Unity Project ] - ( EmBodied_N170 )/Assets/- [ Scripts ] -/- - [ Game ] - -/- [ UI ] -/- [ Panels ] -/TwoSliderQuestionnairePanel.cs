using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
    public class TwoSliderQuestionnairePanel : QuestionnairePanel_Interface<QuestionnairePanel_TwoSliders>
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
        
        
        
        public override void SetupPanel(ref QuestionnairePanel_TwoSliders questionnairePanel)
        {
            _questionOneText.text = questionnairePanel.sliderOneQuestion;
            switch (questionnairePanel.sliderOneQuestionDecorType)
            {
                case QuestionDecor_Enum.Text:
                {
                    _questionOneMinText.gameObject.SetActive(true);
                    _questionOneMaxText.gameObject.SetActive(true);
                    _questionOneMinSprite.gameObject.SetActive(false);
                    _questionOneMaxSprite.gameObject.SetActive(false);
                    
                    _questionOneMinText.text = questionnairePanel.sliderOneQuestionDecorMinimumText;
                    _questionOneMaxText.text = questionnairePanel.sliderOneQuestionDecorMaximumText;
                    
                } break;
                case QuestionDecor_Enum.Image:
                {
                    _questionOneMinText.gameObject.SetActive(false);
                    _questionOneMaxText.gameObject.SetActive(false);
                    _questionOneMinSprite.gameObject.SetActive(true);
                    _questionOneMaxSprite.gameObject.SetActive(true);

                    Texture2D minTexture = questionnairePanel.sliderOneQuestionDecorMinimumSprite, maxTexture = questionnairePanel.sliderOneQuestionDecorMaximumSprite;
                    if (minTexture != null) _questionOneMinSprite.overrideSprite = Sprite.Create(minTexture, new Rect(0, 0, minTexture.width, minTexture.height), new Vector2(0.5f, 0.5f));
                    if (maxTexture != null) _questionOneMaxSprite.overrideSprite = Sprite.Create(maxTexture, new Rect(0, 0, maxTexture.width, maxTexture.height), new Vector2(0.5f, 0.5f));
                } break;
            }

            _questionOneSlider.value = (_questionOneSlider.maxValue + _questionOneSlider.minValue) / 2f;

            _questionTwoText.text = questionnairePanel.sliderTwoQuestion;
            switch (questionnairePanel.sliderTwoQuestionDecorType)
            {
                case QuestionDecor_Enum.Text:
                {
                    _questionTwoMinText.gameObject.SetActive(true);
                    _questionTwoMaxText.gameObject.SetActive(true);
                    _questionTwoMinSprite.gameObject.SetActive(false);
                    _questionTwoMaxSprite.gameObject.SetActive(false);
                    
                    _questionTwoMinText.text = questionnairePanel.sliderTwoQuestionDecorMinimumText;
                    _questionTwoMaxText.text = questionnairePanel.sliderTwoQuestionDecorMaximumText;
                    
                } break;
                case QuestionDecor_Enum.Image:
                {
                    _questionTwoMinText.gameObject.SetActive(false);
                    _questionTwoMaxText.gameObject.SetActive(false);
                    _questionTwoMinSprite.gameObject.SetActive(true);
                    _questionTwoMaxSprite.gameObject.SetActive(true);

                    Texture2D minTexture = questionnairePanel.sliderTwoQuestionDecorMinimumSprite, maxTexture = questionnairePanel.sliderTwoQuestionDecorMaximumSprite;
                    if (minTexture != null) _questionTwoMinSprite.overrideSprite = Sprite.Create(minTexture, new Rect(0, 0, minTexture.width, minTexture.height), new Vector2(0.5f, 0.5f));
                    if (maxTexture != null) _questionTwoMaxSprite.overrideSprite = Sprite.Create(maxTexture, new Rect(0, 0, maxTexture.width, maxTexture.height), new Vector2(0.5f, 0.5f));
                } break;
            }

            _questionTwoSlider.value = (_questionTwoSlider.maxValue + _questionTwoSlider.minValue) / 2f;
        }

        protected override void WriteResultToDataContainer()
        {
            Debug.Log("<color=#2CCED3> Writing Question Data </color>");
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new Data.DataCollection.SliderQuestionData()
            {
                question = _questionOneText.text,
                answer = _questionOneSlider.value
            });
            
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new Data.DataCollection.SliderQuestionData()
            {
                question = _questionTwoText.text,
                answer = _questionTwoSlider.value
            });
        }
    }
}