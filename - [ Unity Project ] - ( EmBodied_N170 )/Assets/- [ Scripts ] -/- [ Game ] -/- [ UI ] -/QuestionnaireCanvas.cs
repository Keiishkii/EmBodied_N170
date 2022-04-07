using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Questionnaire
{
    public class QuestionnaireCanvas : MonoBehaviour
    {
        private GameControllerStateMachine _stateMachine;

        private int _currentQuestionnaireIndex;

        private GameObject _activePanel;

        #region Panels

        [SerializeField] private GameObject _oneQuestionSliderAnswerPanel;

        //private OneQuestionSliderAnswerPanel _oneQuestionSliderAnswerPanelScript;
        [SerializeField] private GameObject _twoQuestionSliderAnswerPanel;
        private TwoQuestionSliderAnswerPanel _twoQuestionSliderAnswerPanelScript;

        #endregion



        private void Awake()
        {
            //_oneQuestionSliderAnswerPanelScript = _oneQuestionSliderAnswerPanel.GetComponent<OneQuestionSliderAnswerPanel>();
            _twoQuestionSliderAnswerPanelScript = _twoQuestionSliderAnswerPanel.GetComponent<TwoQuestionSliderAnswerPanel>();
        }

        public void EnableCanvas(ref GameControllerStateMachine stateMachine)
        {
            _currentQuestionnaireIndex = 0;
            _stateMachine = stateMachine;
            _activePanel = null;

            SetPanelContents();
        }

        private void SetPanelContents()
        {
            if (_stateMachine.currentBlock.blockQuestions.Count > _currentQuestionnaireIndex)
            {
                switch (_stateMachine.currentBlock.blockQuestions[_currentQuestionnaireIndex])
                {
                    case BlockQuestion_TwoQuestionSliderAnswer question:
                    {
                        _activePanel = _twoQuestionSliderAnswerPanel;

                        _twoQuestionSliderAnswerPanel.SetActive(true);
                        _twoQuestionSliderAnswerPanelScript.SetupPanel(question);
                    }
                        break;
                }
            }
            else
            {
                State_Questionnaire.QuestionnaireComplete.Invoke();
            }
        }

        public void OnQuestionAnswered()
        {
            _currentQuestionnaireIndex++;
            _activePanel.SetActive(false);

            SetPanelContents();
        }
    }
}