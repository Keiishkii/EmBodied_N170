using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace NPC_Controller
{
    public class NPCController_StateMachine : MonoBehaviour
    {
        private NPCState_Interface _currentState;

        public NPCState_Interface SetState
        {
            set
            {
                if (_currentState != value)
                {
                    _currentState?.OnExitState(this);
                    _currentState = value;
                    _currentState.OnEnterState(this);
                }
            }
        }

        public readonly NPCState_Idling IdleState = new NPCState_Idling();
        public readonly NPCState_LookingAtPlayer LookingAtPlayer = new NPCState_LookingAtPlayer();



        private NPCIKReferences _npcIKReferences;
        public NPCIKReferences NPCIKReferences => _npcIKReferences ?? (_npcIKReferences = GetComponent<NPCIKReferences>());

        private NPCBoneReferences _npcBoneReferences;
        public NPCBoneReferences NPCBoneReferences => _npcBoneReferences ?? (_npcBoneReferences = GetComponent<NPCBoneReferences>());

        private PlayerController _playerController;
        public PlayerController PlayerController => _playerController ?? (_playerController = FindObjectOfType<PlayerController>());

        private GameControllerStateMachine _gameController;
        public GameControllerStateMachine GameController => _gameController ?? (_gameController = FindObjectOfType<GameControllerStateMachine>());

        
        [SerializeField] private Transform _lookAtTarget;
        public Transform LookAtTarget => _lookAtTarget;
        
        [Space]
        [SerializeField] private float _lookSpeed;
        public float LookSpeed => _lookSpeed;
        
        [SerializeField] private AnimationCurve _lookAtMotionCurve;
        public AnimationCurve LookAtMotionCurve => _lookAtMotionCurve;

        
        
        

        private void Start()
        {
            SetState = IdleState;
        }

        private void Update()
        {
            _currentState.Update(this);
        }
    }
}