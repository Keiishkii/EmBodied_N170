
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
                stateMachine.SetState = stateMachine.IdleState;
            }
        }

        public override void OnExitState(in NPCController_StateMachine stateMachine)
        {
            if (_lookAtTarget != null) stateMachine.StopCoroutine(_lookCoroutine);
        }
        
        
        
        
        
        private IEnumerator LookAtPlayer(NPCController_StateMachine stateMachine)
        {
            Vector3 targetPosition = _playerHeadPosition.position + new Vector3(0, -0.15f, 0);
            Vector3 oldLookTargetPosition = _lookAtTarget.position;
            
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * (1.0f / 0.5f))
            {
                targetPosition = _playerHeadPosition.position + new Vector3(0, -0.15f, 0);
                
                float lerp = stateMachine.LookAtMotionCurve.Evaluate(t);
                _lookAtTarget.position = Vector3.Lerp(oldLookTargetPosition, targetPosition, lerp);
                
                yield return null;
            }

            while (Application.isPlaying)
            {
                targetPosition = _playerHeadPosition.position + new Vector3(0, -0.15f, 0);
                _lookAtTarget.position = targetPosition;
                
                yield return null;
            }
        }
    }
}