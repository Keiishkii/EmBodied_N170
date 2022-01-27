using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItemCollection : MonoBehaviour
{
    private Transform _transform;
    
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffset;

    [SerializeField] private Room_Enum _room;
    private BoxCollider deskCollider;
    
    
    
    
    private void Awake()
    {
        _transform = transform;
        
        deskCollider = GetComponent<BoxCollider>();
        deskCollider.enabled = false;
    }

    private void OnEnable()
    {
        GameController.Instance.EnableColliders.AddListener(EnableDeskCollider);
        GameController.Instance.DisableColliders.AddListener(DisableDeskCollider);
    }

    private void OnDisable()
    {
        GameController.Instance.EnableColliders.RemoveListener(EnableDeskCollider);
        GameController.Instance.DisableColliders.RemoveListener(DisableDeskCollider);
    }



    private void EnableDeskCollider(Room_Enum room)
    {
        if (_room == room)
        {
            deskCollider.enabled = true;
        }
    }
    
    private void DisableDeskCollider()
    {
        deskCollider.enabled = false;
    }





    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        other.transform.SetParent(_transform);
        
        other.transform.localPosition = _positionOffset;
        other.transform.localRotation = Quaternion.Euler(_rotationOffset);
        
        GameController.Instance.GameState = GameState_Enum.PLAYER_DECISION;
    }

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + _positionOffset, 0.025f);
    }
}
