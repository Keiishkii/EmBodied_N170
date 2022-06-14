using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _leftHandTransform, _rightHandTransform;

    [Space]
    [SerializeField] private InputActionReference _rightHandActivation;
    [SerializeField] private InputActionReference _leftHandActivation;
    
    private Enums.Handedness _activeHand = Enums.Handedness.RIGHT;
    
    private GameObject _heldItem;
    private Collider _heldItemCollider;
    
    
    
    
    
    public void CreateHeldItem(GameObject heldObject)
    {
        _heldItem = Instantiate(heldObject, (_activeHand == Enums.Handedness.RIGHT) ? _rightHandTransform : _leftHandTransform);
        _heldItemCollider = _heldItem.GetComponent<Collider>();

        _heldItemCollider.enabled = false;

        //heldObject.transform.localPosition = positionOffset;
        //heldObject.transform.rotation = Quaternion.Euler(eulerRotationOffset);


        if (_activeHand == Enums.Handedness.RIGHT)
        {
            _rightHandActivation.action.started += RightHandActivation;
            _rightHandActivation.action.canceled += RightHandEnded;
        }
        else
        {
            _leftHandActivation.action.started += LeftHandActivation;
            _leftHandActivation.action.canceled += LeftHandEnded;
        }
    }
    
    public void DestroyHeldItem()
    {
        Destroy(_heldItem);


        if (_activeHand == Enums.Handedness.RIGHT)
        {
            _rightHandActivation.action.started -= RightHandActivation;
            _rightHandActivation.action.canceled -= RightHandEnded;
        }
        else
        {
            _leftHandActivation.action.started -= LeftHandActivation;
            _leftHandActivation.action.canceled -= LeftHandEnded;
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