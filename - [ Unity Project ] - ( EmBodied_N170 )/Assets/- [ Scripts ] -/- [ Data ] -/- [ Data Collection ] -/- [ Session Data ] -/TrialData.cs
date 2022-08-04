using System;
using System.Collections.Generic;

namespace Data {   
    namespace DataCollection
    {
        public class TrialData
        {
            public List<QuestionnaireData_Interface> QuestionnaireData = new List<QuestionnaireData_Interface>();

            public string roomACharacterName;
            public int roomACharacterID;

            public string roomBCharacterName;
            public int roomBCharacterID;
        }
    }
}