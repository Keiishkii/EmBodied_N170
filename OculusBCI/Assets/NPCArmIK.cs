using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCArmIK : MonoBehaviour
{
    [SerializeField] private Transform _elbowTransform;
    private Vector3 _elbowPosition;

    private void FixedUpdate()
    {
        _elbowPosition = _elbowTransform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_elbowPosition, 0.025f);
    }
}
