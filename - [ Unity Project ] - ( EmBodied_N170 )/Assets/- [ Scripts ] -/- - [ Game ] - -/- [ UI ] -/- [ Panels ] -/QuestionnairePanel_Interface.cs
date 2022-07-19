using UnityEngine;

namespace Questionnaire
{
    public abstract class QuestionnairePanel_Interface<T> : MonoBehaviour
    {
        private MainCanvas _mainCanvas;
        protected MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = FindObjectOfType<MainCanvas>());
        
        private Data.DataCollection.DataCollector _dataCollector;
        protected Data.DataCollection.DataCollector DataCollector => _dataCollector ?? (_dataCollector = FindObjectOfType<Data.DataCollection.DataCollector>());


        
        public abstract void SetupPanel(ref T question);
        protected abstract void WriteResultToDataContainer();
        
        
        
        public void OnNextButtonPressed()
        {
            WriteResultToDataContainer();
            MainCanvas.OnQuestionAnswered();
        }
    }
}