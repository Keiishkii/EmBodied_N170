using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class NPCIKReferences : MonoBehaviour
{
    public LookAtConstraint 
        lookAtConstraint;
    
    
    public TwoBoneIKConstraint
        leftFootIKConstraint,
        rightFootIKConstraint,
        leftHandIKConstraint,
        rightHandIKConstraint;

    public Transform
        lookAtTarget,
        
        leftFootIKTarget,
        leftFootIKHint,

        rightFootIKTarget,
        rightFootIKHint,
        
        leftHandIKTarget,
        leftHandIKHint,

        rightHandIKTarget,
        rightHandIKHint;
    
}
