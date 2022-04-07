using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementCollider : MonoBehaviour
{
    [SerializeField] private Enums.PlacementChoice _placementChoice;
    
    
    private void OnTriggerEnter(Collider other)
    {
        OnObjectInPlacementZone(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnObjectInPlacementZone(other);
    }


    private void OnObjectInPlacementZone(Collider other)
    {
        StateMachine.State_AwaitingPlayerChoice.ColliderEntered.Invoke(_placementChoice);
    }
}
 