using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class NPCIKReferences : MonoBehaviour
{
    public LookAtConstraint lookAtConstraint;
    [HideInInspector] public Transform lookAtTarget;

    [Space(20)]
    public TwoBoneIKConstraint leftFootIKConstraint;
    [Space(5)]
    public Transform leftFootIKTarget;
    public Transform leftFootIKHint;

    [Space(20)]
    public TwoBoneIKConstraint rightFootIKConstraint;
    [Space(5)]
    public Transform rightFootIKTarget;
    public Transform rightFootIKHint;

    [Space(20)]
    public TwoBoneIKConstraint leftHandIKConstraint;
    [Space(5)]
    public Transform leftHandIKTarget;
    public Transform leftHandIKHint;
    
    [Space(20)]
    public TwoBoneIKConstraint rightHandIKConstraint;
    [Space(5)]
    public Transform rightHandIKTarget;
    public Transform rightHandIKHint;
}
