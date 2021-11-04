using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScriptOrbit : MonoBehaviour
{
    [SerializeField] private Transform _orbitingTransform;
    [SerializeField] private Vector3 _orbitCentre;
    
    [SerializeField] private float _distanceMultiplier, _speedMultiplier;
    private float _tValue;


    
    private void Awake()
    {
        _orbitingTransform.gameObject.SetActive(true);
    }

    private void Update()
    {
        _orbitingTransform.position = _orbitCentre + new Vector3(Mathf.Sin(_tValue) * _distanceMultiplier, Mathf.Cos(_tValue) * _distanceMultiplier, 0);
        _tValue += Time.deltaTime * _speedMultiplier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.7f, 0.5f);
        Gizmos.DrawSphere(_orbitCentre, 0.25f);
    }
}
