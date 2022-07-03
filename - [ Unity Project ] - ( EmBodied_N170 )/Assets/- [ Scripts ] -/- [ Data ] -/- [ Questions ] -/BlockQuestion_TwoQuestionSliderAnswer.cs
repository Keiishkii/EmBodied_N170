using Enums;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Questionnaire
{
    [System.Serializable]
    //[MovedFromAttribute(true, "", "Assembly-CSharp", "BlockQuestion_TwoQuestionSliderPanel")]
    public class BlockQuestion_TwoQuestionSliderAnswer : BlockQuestion
    {
        public string questionOne;
        public string questionTwo;

        public QuestionDecor questionOneDecorType;
            public string questionOneDecorOne_Text, questionOneDecorTwo_Text;
            public Sprite questionOneDecorOne_Sprite, questionOneDecorTwo_Sprite;
            
        public QuestionDecor questionTwoDecorType;
            public string questionTwoDecorOne_Text, questionTwoDecorTwo_Text;
            public Sprite questionTwoDecorOne_Sprite, questionTwoDecorTwo_Sprite;
    }
}