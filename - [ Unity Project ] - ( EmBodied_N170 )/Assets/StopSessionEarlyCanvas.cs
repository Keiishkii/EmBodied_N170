using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class StopSessionEarlyCanvas : MonoBehaviour
{
    private GameControllerStateMachine _stateMachine;

    [SerializeField] private GameObject _panel;
    public bool SetVisible
    {
        set => _panel.SetActive(value);
    }
    
    
    
    
    
    private void Awake()
    {
        _stateMachine = FindObjectOfType<GameControllerStateMachine>();
    }

    public void OnStopSessionEarlyButtonPressed()
    {
        State_BlockStart.ExitEarly.Invoke(_stateMachine);
    }
}
