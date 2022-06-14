using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class NPCController : MonoBehaviour
{
     [HideInInspector] public Enums.Room room;
     private NPCIKReferences _npcIKReferences;
     
     [SerializeField] private float _lookTransitionDuration = 1.5f;

     [Space] 
     [SerializeField] private AnimationCurve _headMovementAnimationCurve;

     private IEnumerator _activeLookAtCoroutine;
     
     
     
     
     
     private void Awake()
     {
          _npcIKReferences = GetComponent<NPCIKReferences>();
          
          StateMachine.State_AwaitingReturnToCorridor.NPCTrigger.AddListener(LookAt);
          SetLookAtTarget(FindObjectOfType<Camera>().transform);
     }


     private void LookAt(Enums.Room room)
     {
          if (room == this.room)
          {
               StartCoroutine(LookAtTargetCoroutine(1.0f));
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
