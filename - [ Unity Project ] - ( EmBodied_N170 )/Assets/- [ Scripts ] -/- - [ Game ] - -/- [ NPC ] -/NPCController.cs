using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class NPCController : MonoBehaviour
{
     [HideInInspector] public Enums.Room room;
     
     private NPCIKReferences _npcIKReferences;
     private NPCBoneReferences _npcBoneReferences;
     
     [SerializeField] private float _lookTransitionDuration = 1.5f;

     [Space] 
     [SerializeField] private AnimationCurve _headMovementAnimationCurve;
     
     private PlayerController _playerController;
     private PlayerController PlayerController => _playerController ?? (_playerController = FindObjectOfType<PlayerController>());
     
     private GameControllerStateMachine _gameControllerStateMachine;
     private GameControllerStateMachine GameControllerStateMachine => _gameControllerStateMachine ?? (_gameControllerStateMachine = FindObjectOfType<GameControllerStateMachine>());

     private IEnumerator _activeLookAtCoroutine;     
     
     
     
     
     
     private void Awake()
     {
          _npcIKReferences = GetComponent<NPCIKReferences>();
          _npcBoneReferences = GetComponent<NPCBoneReferences>();
          
          SetLookAtTarget(FindObjectOfType<Camera>().transform);
          
          State_AwaitingObjectPlacement.NPCPlacementTrigger.AddListener(ThankThePlayer);
     }

     private void OnDestroy()
     {
          State_AwaitingObjectPlacement.NPCPlacementTrigger.RemoveListener(ThankThePlayer);
     }

     private void Start()
     {
          StartCoroutine(ApproachInteraction());
     }

     
     
     

     private IEnumerator ApproachInteraction()
     {
          float approachSquareDistance = Mathf.Pow(GameControllerStateMachine.SessionFormatObject.approachDistance, 2);
          
          Transform npcHeadTransform = _npcBoneReferences.head;
          Transform playerHeadTransform = PlayerController.cameraTransform;

          bool lookingAtPlayer = false;
          while (Application.isPlaying)
          {
               Vector3 playerHeadPosition = playerHeadTransform.position;
               Vector3 npcHeadPosition = npcHeadTransform.position;

               if (!lookingAtPlayer)
               {
                    if (approachSquareDistance > Vector3.SqrMagnitude(playerHeadPosition - npcHeadPosition))
                    {
                         lookingAtPlayer = true;
                         StartCoroutine(LookAtTargetCoroutine(1.0f));
                    }
               }
               else
               {
                    if (approachSquareDistance < Vector3.SqrMagnitude(playerHeadPosition - npcHeadPosition))
                    {
                         lookingAtPlayer = false;
                         StartCoroutine(LookAtTargetCoroutine(0.0f));
                    }
               }

               yield return null;
          }
     }

     
     
     

     private void ThankThePlayer(Enums.Room room)
     {
          if (room == this.room)
          {
               Debug.Log("Thank you!");
          }
     }
     
     private IEnumerator LookAtTargetCoroutine(float targetLookWeight)
     {
          LookAtConstraint lookAtConstraint = _npcIKReferences.lookAtConstraint;
          float currentLookWeight = lookAtConstraint.weight;
          
          float timeElapsed = 0;
          while (timeElapsed < _lookTransitionDuration)
          {
               float iLerp = Mathf.InverseLerp(0, _lookTransitionDuration, timeElapsed);
               float lerp = Mathf.Lerp(currentLookWeight, targetLookWeight, iLerp);
               lookAtConstraint.weight = _headMovementAnimationCurve.Evaluate(lerp);
               
               timeElapsed += Time.deltaTime;
               yield return null;
          }

          lookAtConstraint.weight = targetLookWeight;
          yield return null;
     }

     private void SetLookAtTarget(Transform target)
     {
          _npcIKReferences.lookAtConstraint.AddSource( new ConstraintSource(){ sourceTransform = target, weight = 1 });
     }
}
