using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractionController : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset;
    
    [SerializeField] private GameObject _leftController;
    private XRRayInteractor _leftControllerXRRayInteractor;
    private LineRenderer _leftControllerLineRenderer;
    
    [SerializeField] private GameObject _rightController;
    private XRRayInteractor _rightControllerXRRayInteractor;
    private LineRenderer _rightControllerLineRenderer;
    
    private bool _rayVisibility = false;
    public bool RayVisibility
    {
        set
        {
            _rayVisibility = value;
            UpdateLaserVisibility();
        }
    }
    
    private Enums.Handedness _activeHand = Enums.Handedness.RIGHT;

    [Space]
    [SerializeField] private InputActionReference _rightHandActivate;
    [SerializeField] private InputActionReference _leftHandActivate;
    
    
    
    private void Awake()
    {
        _leftControllerXRRayInteractor = _leftController.GetComponent<XRRayInteractor>();
        _leftControllerLineRenderer = _leftController.GetComponent<LineRenderer>();
        
        _rightControllerXRRayInteractor = _rightController.GetComponent<XRRayInteractor>();
        _rightControllerLineRenderer = _rightController.GetComponent<LineRenderer>();
    }

    
    
    

    // Start is called before the first frame update
    private void OnEnable()
    {
        _rightHandActivate.action.performed += RightHandActivation;
        _leftHandActivate.action.performed += LeftHandActivation;
    }

    private void OnDisable()
    {
        _rightHandActivate.action.performed -= RightHandActivation;
        _leftHandActivate.action.performed -= LeftHandActivation;
    }

    

    
    
    private void RightHandActivation(InputAction.CallbackContext context)
    {
        _activeHand = Enums.Handedness.RIGHT;
        UpdateLaserVisibility();
    }
    
    private void LeftHandActivation(InputAction.CallbackContext context)
    {
        _activeHand = Enums.Handedness.LEFT;
        UpdateLaserVisibility();
    }

    private void UpdateLaserVisibility()
    {
        if (_rayVisibility)
        {
            switch (_activeHand)
            {
                case Enums.Handedness.LEFT:
                {
                    _leftControllerLineRenderer.enabled = true;
                    _leftControllerXRRayInteractor.enabled = true;
                    
                    _rightControllerLineRenderer.enabled = false;
                    _rightControllerXRRayInteractor.enabled = false;
                } break;
                case Enums.Handedness.RIGHT:
                {
                    _leftControllerLineRenderer.enabled = false;
                    _leftControllerXRRayInteractor.enabled = false;
                    
                    _rightControllerLineRenderer.enabled = true;
                    _rightControllerXRRayInteractor.enabled = true;
                } break;
            }
        }
        else
        {
            _leftControllerLineRenderer.enabled = false;
            _leftControllerXRRayInteractor.enabled = false;
            
            _rightControllerLineRenderer.enabled = false;
            _rightControllerXRRayInteractor.enabled = false;
        }
    }
}
