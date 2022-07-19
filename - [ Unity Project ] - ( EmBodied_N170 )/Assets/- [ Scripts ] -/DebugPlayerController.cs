using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugPlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _ascend, _descend;
    [SerializeField] private InputActionReference _forward, _backward, _left, _right;
    [SerializeField] private InputActionReference _orientation;

    [Space]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _mouseSpeed = 1.0f;
    [SerializeField] private float _movementSpeed = 1.0f;

    private Vector2 _offsets = Vector2.zero;
    private const float _maxHeightOffset = 1.35f;
    
    private Quaternion _desiredAvatarRotation;
    private Vector3 _desiredAvatarPosition;

    
    
    
    
    private void Awake()
    {
        _cameraTransform.position += new Vector3(0, 1.5f, 0);
        Cursor.lockState = CursorLockMode.Locked;
        
        SubscribeToInputEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToInputEvents();
    }


    
    private void FixedUpdate()
    {
        ProcessPositionInputs();
        
        _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _desiredAvatarRotation, 0.5f);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _desiredAvatarPosition, 0.05f);
    }

    private void ProcessPositionInputs()
    {
        Vector3 deltaPositionXZ = Vector3.Normalize(new Vector3()
        {
            x = (((_right.action.ReadValue<float>() > 0) ? 1.0f : 0) + ((_left.action.ReadValue<float>() > 0) ? -1.0f : 0)),
            z = (((_forward.action.ReadValue<float>() > 0) ? 1.0f : 0) + ((_backward.action.ReadValue<float>() > 0) ? -1.0f : 0))
        });

        Vector3 deltaPositionY = new Vector3()
        {
            y = (((_ascend.action.ReadValue<float>() > 0) ? 1.0f : 0) + ((_descend.action.ReadValue<float>() > 0) ? -1.0f : 0))
        };
        
        _desiredAvatarPosition = 
            _cameraTransform.position + 
            Vector3.Normalize(deltaPositionY) * _movementSpeed + 
            Vector3.Normalize(_cameraTransform.rotation * deltaPositionXZ) * _movementSpeed;
        
        
    }

    

    private void SubscribeToInputEvents()
    {
        _orientation.action.started += OnMouseDeltaUpdate;
    }
    
    // Removes the action subscription from the mouse delta update event
    private void UnsubscribeToInputEvents()
    {
        _orientation.action.started -= OnMouseDeltaUpdate;
    }
    
    
    
    private void OnMouseDeltaUpdate(InputAction.CallbackContext value)
    {
        Vector2 mouseInput = value.ReadValue<Vector2>();
        if (Mathf.Abs(mouseInput.sqrMagnitude) > 0)
        {
            mouseInput *= _mouseSpeed;
            _offsets += mouseInput;
            _offsets.y = Mathf.Clamp(_offsets.y, -_maxHeightOffset, _maxHeightOffset);

            Vector3 lookDirection = new Vector3(
                Mathf.Sin(_offsets.x) * Mathf.Cos(_offsets.y),
                Mathf.Sin(_offsets.y),
                Mathf.Cos(_offsets.x) * Mathf.Cos(_offsets.y)
            );

            _desiredAvatarRotation = Quaternion.LookRotation(Vector3.Normalize(lookDirection));
        }
    }
}
