using DataCollection;
using StateMachine;
using UnityEngine;

namespace Questionnaire
{
    public abstract class QuestionPanel_Interface<T> : MonoBehaviour
    {
        private MainCanvas _mainCanvas;
        protected MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = FindObjectOfType<MainCanvas>());
        
        private DataCollector _dataCollector;
        protected DataCollector DataCollector => _dataCollector ?? (_dataCollector = FindObjectOfType<DataCollector>());


        
        public abstract void SetupPanel(ref T question);
        protected abstract void WriteResultToDataContainer();
        
        
        
        public void OnNextButtonPressed()
        {
            WriteResultToDataContainer();
            MainCanvas.OnQuestionAnswered();
        }
    }
}