using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementCollider : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        OnObjectInPlacementZone(other);
    }


    private void OnObjectInPlacementZone(Collider other)
    {
        StateMachine.State_AwaitingPlayerChoice.ColliderEntered.Invoke();
    }
}
 