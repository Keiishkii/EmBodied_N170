using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class NPCController : MonoBehaviour
{
     private NPCBoneReferences _npcBoneReferences;
     private NPCIKReferences _npcIKReferences;
     
     [SerializeField] private float _lookTransitionDuration = 1.5f;

     [Space] 
     [SerializeField] private AnimationCurve _headMovementAnimationCurve;

     private IEnumerator _activeLookAtCoroutine;
     private bool _lookingUp = false;     
     
     
     
     
     
     private void Awake()
     {
          _npcBoneReferences = GetComponent<NPCBoneReferences>();
          _npcIKReferences = GetComponent<NPCIKReferences>();
     }
     
     
     
     
     public void LookAt(bool lookAt)
     {
          if (!ReferenceEquals(_activeLookAtCoroutine, null)) 
               StopCoroutine(_activeLookAtCoroutine);

          _activeLookAtCoroutine = LookAtTargetCoroutine(lookAt ? 1.0f : 0.0f);

          StartCoroutine(_activeLookAtCoroutine);
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

     public void SetLookAtTarget(Transform target)
     {
          _npcIKReferences.lookAtConstraint.AddSource( new ConstraintSource(){ sourceTransform = target, weight = 1 });
     }
     
     public Transform GetHeadTransform()
     {
          return _npcBoneReferences.head;
     }
}
