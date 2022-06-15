using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class ObjectPlacementCollider : MonoBehaviour
{
    private Transform _transform;

    private GameControllerStateMachine _gameControllerStateMachine;
    private GameControllerStateMachine GameControllerStateMachine => _gameControllerStateMachine ?? (_gameControllerStateMachine = GameObject.FindObjectOfType<GameControllerStateMachine>());
    
    
    private void Awake()
    {
        _transform = transform;
    }
    
    

    private void OnTriggerStay(Collider other)
    {
        OnObjectInPlacementZone(other);
    }

    private void OnObjectInPlacementZone(Collider other)
    {
        StateMachine.State_AwaitingObjectPlacement.ColliderEntered.Invoke(GameControllerStateMachine);

        Transform objectTransform = other.transform;
        HeldItemProperties properties = other.GetComponent<HeldItemProperties>();
        
        objectTransform.SetParent(_transform.parent);
        
        objectTransform.localPosition = properties.placementPositionOffset;
        objectTransform.localRotation = Quaternion.Euler(properties.placementEulerRotationOffset);
    }
}
 