using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace Questionnaire
{
    public class MainCanvas : MonoBehaviour
    {
        private GameControllerStateMachine _gameControllerStateMachine;
        private GameControllerStateMachine GameControllerStateMachine => _gameControllerStateMachine ?? (_gameControllerStateMachine = GameObject.FindObjectOfType<GameControllerStateMachine>());

        private int _currentQuestionnaireIndex;

        private GameObject _activePanel;

        #region Panels
            [SerializeField] private GameObject _awaitingSessionStartPanel;
            public bool AwaitingSessionStartPanelVisible
            {
                set => _awaitingSessionStartPanel.SetActive(value);
            }
            
            [SerializeField] private GameObject _awaitingBlockStartPanel;
            public bool AwaitingBlockStartPanelVisible
            {
                set => _awaitingBlockStartPanel.SetActive(value);
            }
            
            [SerializeField] private GameObject _readyPanel;
            public bool ReadyPanelVisible
            {
                set => _readyPanel.SetActive(value);
            }
            
            [SerializeField] private GameObject _blockCompletePanel;
            public bool BlockCompletePanelVisible
            {
                set => _blockCompletePanel.SetActive(value);
            }
            
            [SerializeField] private GameObject _sessionCompletePanel;
            public bool SessionCompletePanelVisible
            {
                set => _sessionCompletePanel.SetActive(value);
            }
            
            [Space]
            [SerializeField] private GameObject _oneQuestionSliderAnswerPanel;
            private OneQuestionSliderAnswerPanel _oneQuestionSliderAnswerPanelScript;
            
            [SerializeField] private GameObject _twoQuestionSliderAnswerPanel;
            private TwoQuestionSliderAnswerPanel _twoQuestionSliderAnswerPanelScript;
        #endregion

        [Space] 
        [SerializeField] private GameObject lookTarget;
        public GameObject LookTarget => lookTarget;


        private void Awake()
        {
            _oneQuestionSliderAnswerPanelScript = _oneQuestionSliderAnswerPanel.GetComponent<OneQuestionSliderAnswerPanel>();
            _twoQuestionSliderAnswerPanelScript = _twoQuestionSliderAnswerPanel.GetComponent<TwoQuestionSliderAnswerPanel>();
            
            CanvasSetup();
        }

        private void CanvasSetup()
        {
            AwaitingSessionStartPanelVisible = false;
            AwaitingBlockStartPanelVisible = false;
            ReadyPanelVisible = false;
            BlockCompletePanelVisible = false;
            SessionCompletePanelVisible = false;
        }





        public void OnSessionStartButtonPressed()
        {
            State_SessionStart.StartSession.Invoke(GameControllerStateMachine);
        }
        
        public void OnBlockStartButtonPressed()
        {
            State_BlockStart.StartBlock.Invoke(GameControllerStateMachine);
        }
        
        
        
        
        
        
        
        
     
        
        public void BeginQuestionnaire()
        {
            _currentQuestionnaireIndex = 0;
            _activePanel = null;

            SetPanelContents();
        }

        private void SetPanelContents()
        {
            if (GameControllerStateMachine.currentBlock.blockQuestions.Count > _currentQuestionnaireIndex)
            {
                switch (GameControllerStateMachine.currentBlock.blockQuestions[_currentQuestionnaireIndex])
                {
                    case BlockQuestion_TwoQuestionSliderAnswer question:
                    {
                        _activePanel = _twoQuestionSliderAnswerPanel;

                        _twoQuestionSliderAnswerPanel.SetActive(true);
                        _twoQuestionSliderAnswerPanelScript.SetupPanel(ref question);
                    } break;
                    case BlockQuestion_OneQuestionSliderAnswer question:
                    {
                        _activePanel = _twoQuestionSliderAnswerPanel;

                        _oneQuestionSliderAnswerPanel.SetActive(true);
                        _oneQuestionSliderAnswerPanelScript.SetupPanel(ref question);
                    } break;
                }
            }
            else
            {
                State_Questionnaire.QuestionnaireComplete.Invoke(GameControllerStateMachine);
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