using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataCollection
{
    public class AnswersContainer
    {
        public readonly List<Answer> answers = new List<Answer>();
    }
    
    public class Answer
    {
        public string answer;
    }
}