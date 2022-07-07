
using System.Collections;
using UnityEngine;

namespace NPC_Controller
{
    public class NPCState_LookingAtPlayer : NPCState_Interface
    {
        private Transform _avatarHeadPosition;
        private Transform _playerHeadPosition;
        private Transform _lookAtTarget;

        private float _squareApproachDistance;
        
        private IEnumerator _lookCoroutine;
        
        
        
        
        
        public override void OnEnterState(in NPCController_StateMachine stateMachine)
        {
            _avatarHeadPosition = stateMachine.NPCBoneReferences.head;
            _playerHeadPosition = stateMachine.PlayerController.cameraTransform;
            _lookAtTarget = stateMachine.LookAtTarget;
            
            _squareApproachDistance = Mathf.Pow(stateMachine.GameController.SessionFormatObject.approachDistance, 2);

            _lookCoroutine = LookAtPlayer(stateMachine);
            stateMachine.StartCoroutine(_lookCoroutine);
        }

        public override void Update(in NPCController_StateMachine stateMachine)
        {
            if (Vector3.SqrMagnitude(_avatarHeadPosition.position - _playerHeadPosition.position) > _squareApproachDistance)
            {
                //Debug.Log("<color=#FF0000>END</color>");
                stateMachine.SetState = stateMachine.IdleState;
                return;
            }
        }

        public override void OnExitState(in NPCController_StateMachine stateMachine)
        {
            if (_lookAtTarget != null) stateMachine.StopCoroutine(_lookCoroutine);
        }
        
        
        
        
        
        private IEnumerator LookAtPlayer(NPCController_StateMachine stateMachine)
        {
            Vector3 oldLookTargetPosition = _lookAtTarget.position;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * (1.0f / 0.5f))
            {
                float lerp = stateMachine.LookAtMotionCurve.Evaluate(t);
                _lookAtTarget.position = Vector3.Lerp(oldLookTargetPosition, _playerHeadPosition.position, lerp);
                
                yield return null;
            }

            while (Application.isPlaying)
            {
                _lookAtTarget.position = _playerHeadPosition.position;
                
                yield return null;
            }
        }
    }
}