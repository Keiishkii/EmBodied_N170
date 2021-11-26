using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class NPCController : MonoBehaviour
{
     [HideInInspector] public UnityEvent ToggleLookAtState = new UnityEvent();
     
     [SerializeField] private Transform _lookAtTarget;
     [SerializeField] private float _lookTransitionDuration = 1.5f;

     [Space]
     [SerializeField] private LookAtConstraint _lookAtConstraint;
     [SerializeField] private TwoBoneIKConstraint _leftFootIKConstraint;
     [SerializeField] private TwoBoneIKConstraint _rightFootIKConstraint;

     [Space] 
     [SerializeField] private AnimationCurve _headMovementAnimationCurve;

     private IEnumerator _activeLookAtCoroutine;
     private bool _lookingUp = false;     
     
     
     
     
     
     private void Awake()
     {
          _lookAtConstraint.weight = 0.0f;
          _lookAtConstraint.AddSource( new ConstraintSource(){sourceTransform = _lookAtTarget, weight = 1});
     }

     private void OnEnable()
     {
          ToggleLookAtState.AddListener(ToggleLookAt);
     }

     private void OnDisable()
     {
          ToggleLookAtState.RemoveListener(ToggleLookAt);
     }

     
     
     
     
     private void ToggleLookAt()
     {
          if (!ReferenceEquals(_activeLookAtCoroutine, null)) 
               StopCoroutine(_activeLookAtCoroutine);

          _activeLookAtCoroutine = LookAtTargetCoroutine(_lookingUp ? 0.0f : 1.0f);
          _lookingUp = !_lookingUp;

          StartCoroutine(_activeLookAtCoroutine);
     }
     
     private IEnumerator LookAtTargetCoroutine(float targetLookWeight)
     {
          float currentLookWeight = _lookAtConstraint.weight;
          
          float timeElapsed = 0;
          while (timeElapsed < _lookTransitionDuration)
          {
               float iLerp = Mathf.InverseLerp(0, _lookTransitionDuration, timeElapsed);
               float lerp = Mathf.Lerp(currentLookWeight, targetLookWeight, iLerp);
               _lookAtConstraint.weight = _headMovementAnimationCurve.Evaluate(lerp);
               
               timeElapsed += Time.deltaTime;
               yield return null;
          }

          _lookAtConstraint.weight = targetLookWeight;
          yield return null;
     }
}
