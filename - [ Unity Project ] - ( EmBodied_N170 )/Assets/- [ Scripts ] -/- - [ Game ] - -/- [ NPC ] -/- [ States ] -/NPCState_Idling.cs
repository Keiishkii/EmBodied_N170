using System.Collections;
using UnityEngine;

namespace NPC_Controller
{
    public class NPCState_Idling : NPCState_Interface
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
            
            SetLookTarget(stateMachine, new Vector3(0, 1.5f, 0));
        }

        public override void Update(in NPCController_StateMachine stateMachine)
        {
            if (Vector3.SqrMagnitude(_avatarHeadPosition.position - _playerHeadPosition.position) < _squareApproachDistance)
            {
                stateMachine.SetState = stateMachine.LookingAtPlayer;
                return;
            }
        }

        public override void OnExitState(in NPCController_StateMachine stateMachine)
        {
            if (_lookCoroutine != null) stateMachine.StopCoroutine(_lookCoroutine);
        }

        
        
        

        private void SetLookTarget(in NPCController_StateMachine stateMachine, in Vector3 position)
        {
            if (_lookCoroutine != null) stateMachine.StopCoroutine(_lookCoroutine);
            
            _lookCoroutine = ChangeLookAtTargetPosition(stateMachine, position);
            stateMachine.StartCoroutine(_lookCoroutine);
        }
        
        private IEnumerator ChangeLookAtTargetPosition(NPCController_StateMachine stateMachine, Vector3 position)
        {
            Vector3 oldLookTargetPosition = _lookAtTarget.position;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * (1.0f / 0.5f))
            {
                float lerp = stateMachine.LookAtMotionCurve.Evaluate(t);
                _lookAtTarget.position = Vector3.Lerp(oldLookTargetPosition, position, lerp);
                
                yield return null;
            }
        }
    }
}