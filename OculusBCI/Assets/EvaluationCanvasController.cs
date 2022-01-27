using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvaluationCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    
    [SerializeField] private GameObject _questionPanel;
    [SerializeField] private GameObject _endPanel;

    [SerializeField] private TMP_Text _questionLabel;
    [SerializeField] private TMP_Text _lowAnswerLabel;
    [SerializeField] private TMP_Text _highAnswerLabel;
    
    private List<TestQuestion> _testDataStructure;
    private int _questionID = 0;


    
    
    
    private void Start()
    {
        _canvas.SetActive(false);
    }
    
    
    
    

    public void CanvasEnabled(bool enabled)
    {
        _canvas.SetActive(enabled);
    }

    public void LoadCanvasContent(List<TestQuestion> testDataStructure)
    {
        _testDataStructure = testDataStructure;
        _questionID = 0;
        
        _questionPanel.SetActive(true);
        _endPanel.SetActive(false);
        
        SetQuestion();
    }

    private void SetQuestion()
    {
        if (_questionID < _testDataStructure.Count)
        {
            _questionLabel.text = _testDataStructure[_questionID].question;
            _highAnswerLabel.text = _testDataStructure[_questionID].highAnswer;
            _lowAnswerLabel.text = _testDataStructure[_questionID].lowAnswer;
        }
        else
        {
            _questionPanel.SetActive(false);
            _endPanel.SetActive(true);
        }
    }

    public void OnNextQuestionPressed()
    {
        _questionID++;
        SetQuestion();
        
    }

    public void OnEndQuestioningPressed()
    {
        CanvasEnabled(false);
    }
}
