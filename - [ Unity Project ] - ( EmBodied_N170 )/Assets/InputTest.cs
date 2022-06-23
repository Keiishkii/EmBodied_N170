using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputTest : MonoBehaviour
{
    [SerializeField] private InputActionReference _leftClick;
    [SerializeField] private InputActionReference _rightClick;
    
    private void Awake()
    {
        _leftClick.action.performed += PrintContext;
        _rightClick.action.performed += PrintContext;
    }

    private void OnDestroy()
    {
        _leftClick.action.performed -= PrintContext;
        _rightClick.action.performed -= PrintContext;
    }


    private void PrintContext(InputAction.CallbackContext context)
    {
        Debug.Log($"Context: {context.action.name}");
    }
}
