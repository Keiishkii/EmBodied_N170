using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCorrectionCanvasController : MonoBehaviour
{
    private const float _furthestTransparencyDistance = 2.25f;
    private float _furthestTransparencyDistanceSqr;
    private const float _nearestTransparencyDistance = 1.5f;
    
    private Transform _cameraTransform;
    private Transform CameraTransform => _cameraTransform ?? (_cameraTransform = FindObjectOfType<Camera>().transform);
    
    [SerializeField] private CanvasGroup _roomAWarningCanvasGroup;
    [SerializeField] private CanvasGroup _roomBWarningCanvasGroup;

    private Transform _roomACanvasTransform;
    private Transform _roomBCanvasTransform;

    private bool _roomACanvasVisible;
    public bool RoomACanvasVisible
    {
        set
        {
            _roomAWarningCanvasGroup.alpha = 0;
            _roomACanvasVisible = value;
        }
    }

    private bool _roomBCanvasVisible;
    public bool RoomBCanvasVisible
    {
        set
        {
            _roomBWarningCanvasGroup.alpha = 0;
            _roomBCanvasVisible = value;
        }
    }
    
    
    
    
    
    private void Awake()
    {
        _roomACanvasTransform = _roomAWarningCanvasGroup.transform.parent;
        _roomBCanvasTransform = _roomBWarningCanvasGroup.transform.parent;

        _furthestTransparencyDistanceSqr = Mathf.Pow(_furthestTransparencyDistance, 2);
        
        RoomACanvasVisible = false;
        RoomBCanvasVisible = false;
    }

    
    
    private void Update()
    {
        if (_roomACanvasVisible)
        {
            Vector3 playerPosition = CameraTransform.position, canvasPosition = _roomACanvasTransform.position;
            Vector3 flattenedPlayerPosition = new Vector3()
            {
                x = playerPosition.x,
                y = 0,
                z = playerPosition.z
            };
            
            Vector3 flattenedCanvasPosition = new Vector3()
            {
                x = canvasPosition.x,
                y = 0,
                z = canvasPosition.z
            };

            if (Vector3.SqrMagnitude(flattenedPlayerPosition - flattenedCanvasPosition) < _furthestTransparencyDistanceSqr)
            {
                float distance = Vector3.Distance(
                    flattenedPlayerPosition, 
                    flattenedCanvasPosition
                    );
                
                _roomAWarningCanvasGroup.alpha = Mathf.Clamp01(Mathf.InverseLerp(
                    _furthestTransparencyDistance, 
                    _nearestTransparencyDistance, 
                    distance)
                );
            }
            else { _roomBWarningCanvasGroup.alpha = 0; }
        }
        
        if (_roomBCanvasVisible)
        {
            Vector3 playerPosition = CameraTransform.position, canvasPosition = _roomBCanvasTransform.position;
            Vector3 flattenedPlayerPosition = new Vector3()
            {
                x = playerPosition.x,
                y = 0,
                z = playerPosition.z
            };
            
            Vector3 flattenedCanvasPosition = new Vector3()
            {
                x = canvasPosition.x,
                y = 0,
                z = canvasPosition.z
            };
            
            if (Vector3.SqrMagnitude(flattenedPlayerPosition - flattenedCanvasPosition) < _furthestTransparencyDistanceSqr)
            {
                float distance = Vector3.Distance(
                    flattenedPlayerPosition, 
                    flattenedCanvasPosition
                );
                
                _roomBWarningCanvasGroup.alpha = Mathf.Clamp01(Mathf.InverseLerp(
                    _furthestTransparencyDistance, 
                    _nearestTransparencyDistance, 
                    distance)
                );
            }
            else { _roomBWarningCanvasGroup.alpha = 0; }
        }
    }
}
