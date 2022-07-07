﻿using Enums;
using UnityEngine;

namespace Questionnaire
{
    [System.Serializable]
    public class BlockQuestion_OneQuestionSliderAnswer : BlockQuestion
    {
        // ---- Slider 1
        #region Slider 1
        public string sliderQuestion;
        
        public QuestionDecor_Enum sliderQuestionDecorType;
            public string sliderQuestionDecorMinimumText;
            public string sliderQuestionDecorMaximumText;
            public Texture2D sliderQuestionDecorMinimumSprite;
            public Texture2D sliderQuestionDecorMaximumSprite;
        #endregion
    }
}