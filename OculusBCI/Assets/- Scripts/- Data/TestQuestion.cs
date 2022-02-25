﻿using System;

namespace TestQuestions
{
    [Serializable]
    public class TestQuestion
    {
        public TestQuestionType questionType;
    }

    [Serializable]
    public class SliderGroup
    {
        public string question;
        
        public string lowAnswer;
        public string highAnswer;
    }
    
    
    
    [Serializable]
    public class TestQuestion_1_Slider : TestQuestion
    {
        public SliderGroup question;
    }

    [Serializable]
    public class TestQuestion_2_Slider : TestQuestion
    {
        public SliderGroup questionOne;
        public SliderGroup questionTwo;
    }
}