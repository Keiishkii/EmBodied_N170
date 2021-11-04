using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleTest : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private MeshRenderer _meshRenderer;

    private IEnumerator _returnToOriginalColour;
    
    
    
    private void Awake()
    {
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Select").performed += Grip_WriteToConsole;
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Activate").performed += Trigger_WriteToConsole;
        _actionAsset.FindActionMap("Keyboard").FindAction("WASD").performed += Keyboard_WriteToConsole;
    }

    private void Grip_WriteToConsole(InputAction.CallbackContext context)
    {
        if (_returnToOriginalColour != null) StopCoroutine(_returnToOriginalColour);
        _returnToOriginalColour = ReturnToOriginalColour(new Color(0.2f, 0.4f, 0.9f));
        StartCoroutine(_returnToOriginalColour);
        
        Debug.Log("Grab");
    }
    
    private void Trigger_WriteToConsole(InputAction.CallbackContext context)
    {
        if (_returnToOriginalColour != null) StopCoroutine(_returnToOriginalColour);
        _returnToOriginalColour = ReturnToOriginalColour(new Color(0.2f, 0.9f, 0.4f));
        StartCoroutine(_returnToOriginalColour);
        
        Debug.Log("Trigger");
    }
    
    private void Keyboard_WriteToConsole(InputAction.CallbackContext context)
    {
        if (_returnToOriginalColour != null) StopCoroutine(_returnToOriginalColour);
        _returnToOriginalColour = ReturnToOriginalColour(new Color(0.6f, 0.0f, 0.1f));
        StartCoroutine(_returnToOriginalColour);
        
        Debug.Log("Trigger");
    }

    private IEnumerator ReturnToOriginalColour(Color colour)
    {
        _meshRenderer.material.color = colour;
        
        yield return new WaitForSeconds(0.5f);
        
        _meshRenderer.material.color = Color.white;
    }
}
