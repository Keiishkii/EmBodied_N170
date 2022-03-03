using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using TestQuestions;
using DataCollection;
using QuestionPanels;
using UnityEngine.UI;
using SliderGroup = QuestionPanels.SliderGroup;

public class EvaluationCanvasController : MonoBehaviour
{
    private DataCollector _dataCollector;
    
    [SerializeField] private GameObject _canvas;
    
    [SerializeField] private QuestionPanels.QuestionPanel_1_Slider _questionPanel_1_Slider = new QuestionPanels.QuestionPanel_1_Slider();
    [SerializeField] private QuestionPanels.QuestionPanel_2_Slider _questionPanel_2_Slider = new QuestionPanels.QuestionPanel_2_Slider();
    
    private List<TestQuestions.TestQuestion> _testDataStructure;
    private int _questionID = 0;


    
    
    
    private void Awake()
    {
        _dataCollector = FindObjectOfType<DataCollector>();
    }

    private void Start()
    {
        _canvas.SetActive(false);
        HidePanels();
    }
    
    
    
    

    public void CanvasEnabled(bool enabled)
    {
        _dataCollector.AddNewAnswerGroup();
        _canvas.SetActive(enabled);
    }

    public void LoadCanvasContent(List<TestQuestion> testDataStructure)
    {
        _testDataStructure = testDataStructure;
        _questionID = 0;
        
        SetQuestion();
    }

    private void SetQuestion()
    {
        HidePanels();
        if (_questionID < _testDataStructure.Count)
        {
            switch (_testDataStructure[_questionID])
            {
                case TestQuestion_1_Slider questionGroup:
                {
                    SliderGroup question = _questionPanel_1_Slider.question;
                    question.questionLabel.text = questionGroup.questionGroup.question;
                    question.lowAnswerLabel.text = questionGroup.questionGroup.lowAnswer;
                    question.highAnswerLabel.text = questionGroup.questionGroup.highAnswer;

                    Slider slider = question.slider;
                    slider.value = ((slider.minValue + slider.maxValue) / 2);
                    
                    _questionPanel_1_Slider.panel.SetActive(true);
                } break;
                case TestQuestion_2_Slider questionGroup:
                {
                    SliderGroup question_1 = _questionPanel_2_Slider.questionOne;
                    question_1.questionLabel.text = questionGroup.questionGroupOne.question;
                    question_1.lowAnswerLabel.text = questionGroup.questionGroupOne.lowAnswer;
                    question_1.highAnswerLabel.text = questionGroup.questionGroupOne.highAnswer;
                    
                    Slider slider_1 = question_1.slider;
                    slider_1.value = ((slider_1.minValue + slider_1.maxValue) / 2);
                    
                    
                    SliderGroup question_2 = _questionPanel_2_Slider.questionTwo;
                    question_2.questionLabel.text = questionGroup.questionGroupTwo.question;
                    question_2.lowAnswerLabel.text = questionGroup.questionGroupTwo.lowAnswer;
                    question_2.highAnswerLabel.text = questionGroup.questionGroupTwo.highAnswer;
                    
                    Slider slider_2 = question_2.slider;
                    slider_2.value = ((slider_2.minValue + slider_2.maxValue) / 2);
                    
                    _questionPanel_2_Slider.panel.SetActive(true);
                }break;
            }
        }
        else
        {
            _canvas.SetActive(false);
            
            GameController.Instance.gameState = GameState_Enum.PLAYER_RESULTS_COLLECTED; 
        }
    }

    private void HidePanels()
    {
        _questionPanel_1_Slider.panel.SetActive(false);
        _questionPanel_2_Slider.panel.SetActive(false);
    }
    
    public void OnNextQuestionPressed()
    {
        switch (_testDataStructure[_questionID])
        {
            case TestQuestion_1_Slider questionGroup:
            {
                _dataCollector.AddNewAnswer($"{_questionPanel_1_Slider.question.slider.value}");
            } break;
            case TestQuestion_2_Slider questionGroup:
            {
                _dataCollector.AddNewAnswer($"{_questionPanel_2_Slider.questionOne.slider.value}");
                _dataCollector.AddNewAnswer($"{_questionPanel_2_Slider.questionTwo.slider.value}");
            }break;
        }
        
        _questionID++;
        SetQuestion();
    }
}
