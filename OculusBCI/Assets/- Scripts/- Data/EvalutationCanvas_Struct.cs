using System;
using TMPro;
using UnityEngine;

namespace QuestionPanels
{
    [Serializable]
    public class QuestionPanel
    {
        public GameObject panel;
    }

    [Serializable]
    public class SliderGroup
    {
        public TMP_Text questionLabel;
        public TMP_Text lowerAnswer;
        public TMP_Text upperAnswer;
    }

    

    [Serializable]
    public class QuestionPanel_1_Slider : QuestionPanel
    {
        public SliderGroup question;
    }

    [Serializable]
    public class QuestionPanel_2_Slider : QuestionPanel
    {
        public SliderGroup questionOne;
        public SliderGroup questionTwo;
    }
}