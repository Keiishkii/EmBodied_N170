using System.Collections;
using System.Collections.Generic;
using Questionnaire.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Questionnaire
{
    [System.Serializable]
    public class QuestionnairePanel
    {
        [FormerlySerializedAs("questionType")] public QuestionnairePanelType_Enum questionnairePanelType = QuestionnairePanelType_Enum.Unselected;
    }
}