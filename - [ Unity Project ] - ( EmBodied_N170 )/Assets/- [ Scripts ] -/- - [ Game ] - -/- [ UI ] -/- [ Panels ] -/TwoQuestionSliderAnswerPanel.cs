using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using TMPro;
using UnityEngine;

namespace Questionnaire
{
    public class TwoQuestionSliderAnswerPanel : MonoBehaviour
    {
        private MainCanvas _mainCanvas;
        private GameControllerStateMachine _stateMachine;

        [SerializeField] private TMP_Text _questionOne, _questionTwo;



        private void Awake()
        {
            _mainCanvas = FindObjectOfType<MainCanvas>();
            _stateMachine = FindObjectOfType<GameControllerStateMachine>();
        }

        public void SetupPanel(BlockQuestion_TwoQuestionSliderAnswer question)
        {
            _questionOne.text = question.questionOne;
            _questionTwo.text = question.questionTwo;
        }

        public void OnNextButtonPressed()
        {
            //_stateMachine.dataContainer.blockData[_stateMachine.blockIndex].trialData[_stateMachine.trialIndex].AddQuestion
            _mainCanvas.OnQuestionAnswered();
        }
    }
}