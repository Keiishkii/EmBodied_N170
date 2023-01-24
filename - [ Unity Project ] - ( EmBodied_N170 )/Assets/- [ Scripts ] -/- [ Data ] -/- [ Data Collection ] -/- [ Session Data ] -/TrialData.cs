using System;
using System.Collections.Generic;

namespace Data 
{   
    namespace DataCollection
    {
        /// <summary>
        /// Data container class for storing information on a given trial, this includes the questions shown during the questionnaire phase, if one is included.
        /// And signifiers for determining what characters the player interacted with during the trial.
        /// </summary>
        public class TrialData
        {
            // A list of questions shown to the user and the answers they replied with.
            public List<QuestionnaireData_Interface> QuestionnaireData = new List<QuestionnaireData_Interface>();

            // Description of the non player character within room A the user interacted with during the trial.
            public string roomACharacterName;
            public int roomACharacterID;

            // Description of the non player character within room B the user interacted with during the trial.
            public string roomBCharacterName;
            public int roomBCharacterID;
        }
    }
}