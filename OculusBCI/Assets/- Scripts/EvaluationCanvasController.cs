using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvaluationCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    
    [SerializeField] private QuestionPanels.QuestionPanel_1_Slider _questionPanel_1_Slider = new QuestionPanels.QuestionPanel_1_Slider();
    [SerializeField] private QuestionPanels.QuestionPanel_2_Slider _questionPanel_2_Slider = new QuestionPanels.QuestionPanel_2_Slider();
    
    private List<TestQuestions.TestQuestion> _testDataStructure;
    private int _questionID = 0;


    
    
    
    private void Start()
    {
        _canvas.SetActive(false);
        
        _questionPanel_1_Slider.panel.SetActive(false);
        _questionPanel_2_Slider.panel.SetActive(false);
    }
    
    
    
    

    public void CanvasEnabled(bool enabled)
    {
        _canvas.SetActive(enabled);
    }

    public void LoadCanvasContent(List<TestQuestions.TestQuestion> testDataStructure)
    {
        _testDataStructure = testDataStructure;
        _questionID = 0;
        
        //_questionPanel.SetActive(true);
        
        SetQuestion();
    }

    private void SetQuestion()
    {
        if (_questionID < _testDataStructure.Count)
        {
            //_questionLabel.text = _testDataStructure[_questionID].question;
            //_highAnswerLabel.text = _testDataStructure[_questionID].highAnswer;
            //_lowAnswerLabel.text = _testDataStructure[_questionID].lowAnswer;
        }
        else
        {
            //_questionPanel.SetActive(false);
            _canvas.SetActive(false);
            
            GameController.Instance.gameState = GameState_Enum.PLAYER_RESULTS_COLLECTED; 
        }
    }

    public void OnNextQuestionPressed()
    {
        _questionID++;
        SetQuestion();
    }
}
