using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        public Slider slider;
        
        public TMP_Text questionLabel;
        public TMP_Text lowAnswerLabel;
        public TMP_Text highAnswerLabel;
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