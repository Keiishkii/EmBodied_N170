using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform, leftHandTransform, rightHandTransform;

    [Space]
    [SerializeField] private InputActionReference _rightHandActivation;
    [SerializeField] private InputActionReference _leftHandActivation;
    
    private GameObject _heldItem;
    private Collider _heldItemCollider;

    private Handedness _handedness;
    
    
    
    
    public void CreateHeldItem(in GameObject heldObject, in Handedness handedness)
    {
        _handedness = handedness;
        
        _heldItem = Instantiate(heldObject, (_handedness == Enums.Handedness.Right) ? rightHandTransform : leftHandTransform);
        
        _heldItemCollider = _heldItem.GetComponent<Collider>();
        _heldItemCollider.enabled = false;
        
        HeldItemProperties properties = heldObject.GetComponent<HeldItemProperties>();
        
        switch (_handedness)
        {
            case Handedness.Left:
            {
                _heldItem.transform.localRotation = Quaternion.Euler(properties.leftHandHoldingEulerRotationOffset);
                _heldItem.transform.localPosition = properties.leftHandHoldingPositionOffset;

                    Debug.Log($"Left Hand Offset: {properties.leftHandHoldingPositionOffset}");

                _leftHandActivation.action.started += LeftHandActivation;
                _leftHandActivation.action.canceled += LeftHandEnded;
            } break;
            case Handedness.Right:
            {
                _heldItem.transform.localRotation = Quaternion.Euler(properties.rightHandHoldingEulerRotationOffset);
                _heldItem.transform.localPosition = properties.rightHandHoldingPositionOffset;

                    Debug.Log($"Right Hand Offset: {properties.rightHandHoldingPositionOffset}");

                _rightHandActivation.action.started += RightHandActivation;
                _rightHandActivation.action.canceled += RightHandEnded;
            } break;
        }
    }
    
    public void DestroyHeldItem()
    {
        Destroy(_heldItem);

        switch (_handedness)
        {
            case Handedness.Left:
            {
                _leftHandActivation.action.started -= LeftHandActivation;
                _leftHandActivation.action.canceled -= LeftHandEnded;
            } break;
            case Handedness.Right:
            {
                _rightHandActivation.action.started -= RightHandActivation;
                _rightHandActivation.action.canceled -= RightHandEnded;
            } break;
        }
    }
    
    
    
    
    
    private void RightHandActivation(InputAction.CallbackContext context)
    {
        Debug.Log("Right: Activated");
        _heldItemCollider.enabled = true;
        Debug.Log($"Collider: {_heldItemCollider.enabled}");
    }
    
    private void RightHandEnded(InputAction.CallbackContext context)
    {
        Debug.Log("Right: Ended");
        _heldItemCollider.enabled = false;
        Debug.Log($"Collider: {_heldItemCollider.enabled}");
    }
    
    
    
    private void LeftHandActivation(InputAction.CallbackContext context)
    {
        Debug.Log("Left: Activated");
        _heldItemCollider.enabled = true;
        Debug.Log($"Collider: {_heldItemCollider.enabled}");
    }
    
    private void LeftHandEnded(InputAction.CallbackContext context)
    {
        Debug.Log("Left: Ended");
        _heldItemCollider.enabled = false;
        Debug.Log($"Collider: {_heldItemCollider.enabled}");
    }
}