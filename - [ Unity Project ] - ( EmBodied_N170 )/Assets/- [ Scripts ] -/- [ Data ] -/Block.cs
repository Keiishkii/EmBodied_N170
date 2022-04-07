using System;
using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class Block
    {
        [SerializeReference] public List<Trial> trials = new List<Trial>();
        [SerializeReference] public List<BlockQuestion> blockQuestions = new List<BlockQuestion>();
    }
}
