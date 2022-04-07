using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Enums.Room _room;
    [SerializeField] private GameObject _deskCollider, _tableCollider;

    
    private void OnEnable()
    {
        if (_room == Enums.Room.ROOM_A)
            State_AwaitingPlayerChoice.ActivateRoomAColliders.AddListener(OnColliderActivation);
        else
            State_AwaitingPlayerChoice.ActivateRoomBColliders.AddListener(OnColliderActivation);
    }

    private void OnDisable()
    {
        if (_room == Enums.Room.ROOM_A)
            State_AwaitingPlayerChoice.ActivateRoomAColliders.AddListener(OnColliderActivation);
        else
            State_AwaitingPlayerChoice.ActivateRoomBColliders.RemoveListener(OnColliderActivation);
    }


    private void OnColliderActivation(bool active)
    {
        _deskCollider.SetActive(active);
        _tableCollider.SetActive(active);
    }
}
