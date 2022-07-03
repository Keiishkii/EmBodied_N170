using Enums;
using UnityEngine;

namespace Questionnaire
{
    [System.Serializable]
    public class BlockQuestion_OneQuestionSliderAnswer : BlockQuestion
    {
        public string questionOne;
        
        public QuestionDecor questionOneDecorType;
            public string questionOneDecorOne_Text, questionOneDecorTwo_Text;
            public Sprite questionOneDecorOne_Sprite, questionOneDecorTwo_Sprite;
    }
}