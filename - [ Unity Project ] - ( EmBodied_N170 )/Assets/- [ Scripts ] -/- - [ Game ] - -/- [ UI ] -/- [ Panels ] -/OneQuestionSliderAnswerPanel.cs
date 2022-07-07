using DataCollection;
using Enums;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
    public class OneQuestionSliderAnswerPanel : QuestionPanel_Interface<BlockQuestion_OneQuestionSliderAnswer>
    {
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private Slider _questionSlider;

        [Space(5)]
        [SerializeField] private TMP_Text _questionMinText;
        [SerializeField] private TMP_Text _questionMaxText;
        [SerializeField] private Image _questionMinSprite;
        [SerializeField] private Image _questionMaxSprite;
        
        
        public override void SetupPanel(ref BlockQuestion_OneQuestionSliderAnswer blockQuestion)
        {
            _questionText.text = blockQuestion.sliderQuestion;
            switch (blockQuestion.sliderQuestionDecorType)
            {
                case QuestionDecor_Enum.Text:
                {
                    _questionMinText.gameObject.SetActive(true);
                    _questionMaxText.gameObject.SetActive(true);
                    _questionMinSprite.gameObject.SetActive(false);
                    _questionMaxSprite.gameObject.SetActive(false);
                    
                    _questionMinText.text = blockQuestion.sliderQuestionDecorMinimumText;
                    _questionMaxText.text = blockQuestion.sliderQuestionDecorMaximumText;
                    
                } break;
                case QuestionDecor_Enum.Image:
                {
                    _questionMinText.gameObject.SetActive(false);
                    _questionMaxText.gameObject.SetActive(false);
                    _questionMinSprite.gameObject.SetActive(true);
                    _questionMaxSprite.gameObject.SetActive(true);

                    Texture2D minTexture = blockQuestion.sliderQuestionDecorMinimumSprite, maxTexture = blockQuestion.sliderQuestionDecorMaximumSprite;
                    if (minTexture != null) _questionMinSprite.overrideSprite = Sprite.Create(minTexture, new Rect(0, 0, minTexture.width, minTexture.height), new Vector2(0.5f, 0.5f));
                    if (maxTexture != null) _questionMaxSprite.overrideSprite = Sprite.Create(maxTexture, new Rect(0, 0, maxTexture.width, maxTexture.height), new Vector2(0.5f, 0.5f));
                } break;
            }
        }

        protected override void WriteResultToDataContainer()
        {
            DataCollector.CurrentTrialData.QuestionnaireData.Add(new SliderQuestionData()
            {
                question = _questionText.text,
                answer = _questionSlider.value
            });
        }
    }
}