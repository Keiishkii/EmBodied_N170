using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Enums.Room _room;
    [SerializeField] private GameObject _deskCollider;

    private bool _colliderActive;
    private bool ColliderActive
    {
        set
        {
            _deskCollider.SetActive(value);
            _colliderActive = value;
        }
    }


    private void Awake()
    {
        ColliderActive = false;
    }

    private void OnEnable()
    {
        if (_room == Enums.Room.RoomA)
            State_AwaitingObjectPlacement.ActivateRoomAColliders.AddListener(OnColliderActivation);
        else
            State_AwaitingObjectPlacement.ActivateRoomBColliders.AddListener(OnColliderActivation);
    }

    private void OnDisable()
    {
        if (_room == Enums.Room.RoomA)
            State_AwaitingObjectPlacement.ActivateRoomAColliders.AddListener(OnColliderActivation);
        else
            State_AwaitingObjectPlacement.ActivateRoomBColliders.RemoveListener(OnColliderActivation);
    }


    private void OnColliderActivation(bool activeState)
    {
        ColliderActive = activeState;
    }
}
