using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItemCollection : MonoBehaviour
{
    private Transform _transform;
    
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffset;
    
    
    
    
    
    private void Awake()
    {
        _transform = transform;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        other.transform.SetParent(_transform);
        
        other.transform.localPosition = _positionOffset;
        other.transform.localRotation = Quaternion.Euler(_rotationOffset);
        
        GameController.Instance.SetState(GameState_Enum.PLAYER_INPUT);
    }

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + _positionOffset, 0.025f);
    }
}
