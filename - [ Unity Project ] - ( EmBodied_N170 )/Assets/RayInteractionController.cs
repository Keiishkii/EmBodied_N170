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
    private Handedness _activeHand = Handedness.RIGHT;

    
    
    
    
    private void Awake()
    {
        _leftControllerXRRayInteractor = _leftController.GetComponent<XRRayInteractor>();
        _leftControllerLineRenderer = _leftController.GetComponent<LineRenderer>();
        
        _rightControllerXRRayInteractor = _rightController.GetComponent<XRRayInteractor>();
        _rightControllerLineRenderer = _rightController.GetComponent<LineRenderer>();
    }

    
    
    

    // Start is called before the first frame update
    void OnEnable()
    {
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Select").performed += RightControllerGrip;
        _actionAsset.FindActionMap("XRI LeftHand").FindAction("Select").performed += LeftControllerGrip;
        
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Activate").performed += RightControllerTrigger;
        _actionAsset.FindActionMap("XRI LeftHand").FindAction("Activate").performed += LeftControllerTrigger;
    }

    private void OnDisable()
    {
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Select").performed -= RightControllerGrip;
        _actionAsset.FindActionMap("XRI LeftHand").FindAction("Select").performed -= LeftControllerGrip;
        
        _actionAsset.FindActionMap("XRI RightHand").FindAction("Activate").performed -= RightControllerTrigger;
        _actionAsset.FindActionMap("XRI LeftHand").FindAction("Activate").performed -= LeftControllerTrigger;
    }

    
    


    public void SetLaserVisibility(bool visible)
    {
        _rayVisibility = visible;
        UpdateLaserVisibility();
    }
    
    private void RightControllerTrigger(InputAction.CallbackContext context)
    {
        _activeHand = Handedness.RIGHT;
        UpdateLaserVisibility();
    }
    
    private void RightControllerGrip(InputAction.CallbackContext context)
    {
        _activeHand = Handedness.RIGHT;
        UpdateLaserVisibility();
    }
    
    private void LeftControllerTrigger(InputAction.CallbackContext context)
    {
        _activeHand = Handedness.LEFT;
        UpdateLaserVisibility();
    }
    
    private void LeftControllerGrip(InputAction.CallbackContext context)
    {
        _activeHand = Handedness.LEFT;
        UpdateLaserVisibility();
    }

    private void UpdateLaserVisibility()
    {
        if (_rayVisibility)
        {
            switch (_activeHand)
            {
                case Handedness.LEFT:
                {
                    _leftControllerLineRenderer.enabled = true;
                    _leftControllerXRRayInteractor.enabled = true;
                    
                    _rightControllerLineRenderer.enabled = false;
                    _rightControllerXRRayInteractor.enabled = false;
                } break;
                case Handedness.RIGHT:
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
