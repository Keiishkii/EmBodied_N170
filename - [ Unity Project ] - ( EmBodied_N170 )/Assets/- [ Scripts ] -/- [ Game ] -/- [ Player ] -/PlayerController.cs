using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _leftHandTransform, _rightHandTransform;

    private Enums.Handedness _activeHand = Enums.Handedness.RIGHT;
    
    [Space] 
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 eulerRotationOffset;
    
    private GameObject _heldItem;
    private Collider _heldItemCollider;
    
    
    
    public void CreateHeldItem(GameObject heldObject)
    {
        _heldItem = Instantiate(heldObject, (_activeHand == Enums.Handedness.RIGHT) ? _rightHandTransform : _leftHandTransform);
        _heldItemCollider = heldObject.GetComponent<Collider>();

        _heldItemCollider.enabled = false;
        
        heldObject.transform.localPosition = positionOffset;
        heldObject.transform.rotation = Quaternion.Euler(eulerRotationOffset);
    }
    
    public void DestroyHeldItem()
    {
        Destroy(_heldItem);
    }
}