using Enums;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Questionnaire
{
    [System.Serializable]
    //[MovedFromAttribute(true, "", "Assembly-CSharp", "BlockQuestion_TwoQuestionSliderPanel")]
    public class BlockQuestion_TwoQuestionSliderAnswer : BlockQuestion
    {
        // ---- Slider 1
        #region Slider 1
        public string sliderOneQuestion;
        
        public QuestionDecor_Enum sliderOneQuestionDecorType;
            public string sliderOneQuestionDecorMinimumText;
            public string sliderOneQuestionDecorMaximumText;
            public Texture2D sliderOneQuestionDecorMinimumSprite;
            public Texture2D sliderOneQuestionDecorMaximumSprite;
        #endregion
        
        // ---- Slider 2
        #region Slider 2
        public string sliderTwoQuestion;
        
        public QuestionDecor_Enum sliderTwoQuestionDecorType;
            public string sliderTwoQuestionDecorMinimumText;
            public string sliderTwoQuestionDecorMaximumText;
            public Texture2D sliderTwoQuestionDecorMinimumSprite;
            public Texture2D sliderTwoQuestionDecorMaximumSprite;
        #endregion
    }
}