using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtCursor : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;
    private Transform _cursorTransform;
    
    [SerializeField] private Transform _canvasTransform;
    
    private Transform _transform;

    private bool _cursorActive;
    public bool CursorActive
    {
        set
        {
            if (_cursor.activeInHierarchy != value) _cursor.SetActive(value);
            _cursorActive = value;
        }
        get => _cursorActive;
    }


    
    private void Awake()
    {
        _transform = transform;
        _cursorTransform = _cursor.transform;
        
        CursorActive = false;
    }

    

    private void Update()
    {
        if (CursorActive)
        {
            Vector3 position = _transform.position;
            Vector3 forward = _transform.forward;

            
            Plane canvasPlane = new Plane(_canvasTransform.forward * -1, _canvasTransform.position);
            Ray ray = new Ray(position, forward);

            if (canvasPlane.Raycast(ray, out float distance))
            {
                _cursor.SetActive(true);
                _cursorTransform.position = position + (forward * distance);
            }
            else
            {
                _cursor.SetActive(false);
            }
        }
    }
}
