using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class FiniteStateMachineManager : MonoBehaviour
{
    private readonly FiniteStateMachine _stateMachine = new FiniteStateMachine();

    private void Start()
    {
        _stateMachine.Start();
    }

    private void Update()
    {
        _stateMachine.Update();
    }
}
