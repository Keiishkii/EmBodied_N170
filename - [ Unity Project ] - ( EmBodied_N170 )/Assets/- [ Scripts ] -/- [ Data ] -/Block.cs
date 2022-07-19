using System;
using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using UnityEngine;

namespace Data {
    namespace Input
    {
        [Serializable]
        public class Block
        {
            [SerializeReference] public List<Trial> trials = new List<Trial>();
            [SerializeReference] public List<QuestionnairePanel> blockQuestionnairePanels = new List<QuestionnairePanel>();

            public Enums.Room targetRoom;
        }
    }
}