namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// A class describing the question and answer given from the user, based on a slider based question.
        /// The answer is stored in float form, usually ranging between 0 and 1.
        /// </summary>
        public class SliderQuestionData : QuestionnaireData_Interface
        {
            public string question;
            public float answer;
        }
    }
}