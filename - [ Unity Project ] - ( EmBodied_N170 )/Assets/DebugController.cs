using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset;
    //private GameController _gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        //_gameController = GetComponent<GameController>();
    }
    
    
    
    private void Awake()
    {
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Select").performed += Grip_WriteToConsole;
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Activate").performed += Trigger_WriteToConsole;
        _actionAsset.FindActionMap("Keyboard").FindAction("WASD").performed += Keyboard_WriteToConsole;
    }

    private void Grip_WriteToConsole(InputAction.CallbackContext context)
    {
        //_gameController.gameState++;
        Debug.Log("Grab");
    }
    
    private void Trigger_WriteToConsole(InputAction.CallbackContext context)
    {
        Debug.Log("Trigger");
    }
    
    private void Keyboard_WriteToConsole(InputAction.CallbackContext context)
    {
        //_gameController.gameState++;
        Debug.Log("Trigger");
    }
}
