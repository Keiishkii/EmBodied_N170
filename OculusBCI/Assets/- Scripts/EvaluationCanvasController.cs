using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using TestQuestions;
using DataCollection;

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
                    _questionPanel_1_Slider.question.questionLabel.text = questionGroup.questionGroup.question;
                    _questionPanel_1_Slider.question.lowAnswerLabel.text = questionGroup.questionGroup.lowAnswer;
                    _questionPanel_1_Slider.question.highAnswerLabel.text = questionGroup.questionGroup.highAnswer;
                    
                    _questionPanel_1_Slider.panel.SetActive(true);
                } break;
                case TestQuestion_2_Slider questionGroup:
                {
                    _questionPanel_2_Slider.questionOne.questionLabel.text = questionGroup.questionGroupOne.question;
                    _questionPanel_2_Slider.questionOne.lowAnswerLabel.text = questionGroup.questionGroupOne.lowAnswer;
                    _questionPanel_2_Slider.questionOne.highAnswerLabel.text = questionGroup.questionGroupOne.highAnswer;
                    
                    _questionPanel_2_Slider.questionTwo.questionLabel.text = questionGroup.questionGroupTwo.question;
                    _questionPanel_2_Slider.questionTwo.lowAnswerLabel.text = questionGroup.questionGroupTwo.lowAnswer;
                    _questionPanel_2_Slider.questionTwo.highAnswerLabel.text = questionGroup.questionGroupTwo.highAnswer;
                    
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
    {switch (_testDataStructure[_questionID])
        {
            case TestQuestion_1_Slider questionGroup:
            {
                _dataCollector.AddQuestionResults(questionGroup.questionGroup.question, $"{_questionPanel_1_Slider.question.slider.value}");
            } break;
            case TestQuestion_2_Slider questionGroup:
            {
                _dataCollector.AddQuestionResults(questionGroup.questionGroupOne.question, $"{_questionPanel_2_Slider.questionOne.slider.value}");
                _dataCollector.AddQuestionResults(questionGroup.questionGroupTwo.question, $"{_questionPanel_2_Slider.questionTwo.slider.value}");
            }break;
        }
        
        _questionID++;
        SetQuestion();
    }
}
