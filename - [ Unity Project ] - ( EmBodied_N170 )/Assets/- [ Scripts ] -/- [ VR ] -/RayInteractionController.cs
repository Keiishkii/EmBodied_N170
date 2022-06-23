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
    [SerializeField] private GameObject _leftReticule;
    private XRRayInteractor _leftControllerXRRayInteractor;
    private LineRenderer _leftControllerLineRenderer;
    
    [SerializeField] private GameObject _rightController;
    [SerializeField] private GameObject _rightReticule;
    private XRRayInteractor _rightControllerXRRayInteractor;
    private LineRenderer _rightControllerLineRenderer;
    
    
    private HandAnimationController _handAnimationController;
    private HandAnimationController HandAnimationController => _handAnimationController ?? (_handAnimationController = GameObject.FindObjectOfType<HandAnimationController>());

    
    private bool _rayVisibility = false;
    public bool RayVisibility
    {
        set
        {
            _rayVisibility = value;
            UpdateLaserVisibility();
        }
    }
    
    private Enums.Handedness _activeHand = Enums.Handedness.Right;

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
        _activeHand = Enums.Handedness.Right;
        UpdateLaserVisibility();
    }
    
    private void LeftHandActivation(InputAction.CallbackContext context)
    {
        _activeHand = Enums.Handedness.Left;
        UpdateLaserVisibility();
    }

    private void UpdateLaserVisibility()
    {
        if (_rayVisibility)
        {
            switch (_activeHand)
            {
                case Enums.Handedness.Left:
                {
                    HandAnimationController.LeftHandState = HandAnimationState.Pointing;
                    HandAnimationController.RightHandState = HandAnimationState.Default;
                    
                    _leftControllerLineRenderer.enabled = true;
                    _leftControllerXRRayInteractor.enabled = true;
                    _leftReticule.SetActive(true);
                    
                    _rightControllerLineRenderer.enabled = false;
                    _rightControllerXRRayInteractor.enabled = false;
                    _rightReticule.SetActive(false);
                } break;
                case Enums.Handedness.Right:
                {
                    HandAnimationController.LeftHandState = HandAnimationState.Default;
                    HandAnimationController.RightHandState = HandAnimationState.Pointing;
                    
                    _leftControllerLineRenderer.enabled = false;
                    _leftControllerXRRayInteractor.enabled = false;
                    _leftReticule.SetActive(false);
                    
                    _rightControllerLineRenderer.enabled = true;
                    _rightControllerXRRayInteractor.enabled = true;
                    _rightReticule.SetActive(true);
                } break;
            }
        }
        else
        {
            _leftControllerLineRenderer.enabled = false;
            _leftControllerXRRayInteractor.enabled = false;
            _leftReticule.SetActive(false);
            
            _rightControllerLineRenderer.enabled = false;
            _rightControllerXRRayInteractor.enabled = false;
            _rightReticule.SetActive(false);
        }
    }
}
