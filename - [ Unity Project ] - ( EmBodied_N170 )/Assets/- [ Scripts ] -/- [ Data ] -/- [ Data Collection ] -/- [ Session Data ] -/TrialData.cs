using System;
using System.Collections.Generic;

namespace DataCollection
{
    public class TrialData
    {
        public Enums.Room activeRoom;
        public List<QuestionnaireData_Interface> QuestionnaireData = new List<QuestionnaireData_Interface>();
    }
}