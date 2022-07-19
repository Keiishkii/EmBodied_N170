using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    
    
    
    
    [ContextMenu("Auto Complete 'IK References'")]
    public void FillInIkData()
    {
        lookAtConstraint = GetComponentInChildren<LookAtConstraint>();
        
        List<TwoBoneIKConstraint> entityIKConstraints = GetComponentsInChildren<TwoBoneIKConstraint>().ToList();
        foreach (TwoBoneIKConstraint entityIKConstraint in entityIKConstraints)
        {
            string name = entityIKConstraint.name;
            if (name.Contains("Left"))
            {
                if (name.Contains("Leg"))
                {
                    leftFootIKConstraint = entityIKConstraint;
                }
                else if (name.Contains("Hand"))
                {
                    leftHandIKConstraint = entityIKConstraint;
                }
            }
            else if (name.Contains("Right"))
            {
                if (name.Contains("Leg"))
                {
                    rightFootIKConstraint = entityIKConstraint;
                }
                else if (name.Contains("Hand"))
                {
                    rightHandIKConstraint = entityIKConstraint;
                }
            }
        }
        
        
        FindTargetInChildren(leftFootIKConstraint.transform, out leftFootIKTarget);
        FindHintInChildren(leftFootIKConstraint.transform, out leftFootIKHint);
        
        FindTargetInChildren(rightFootIKConstraint.transform, out rightFootIKTarget);
        FindHintInChildren(rightFootIKConstraint.transform, out rightFootIKHint);
        
        FindTargetInChildren(leftHandIKConstraint.transform, out leftHandIKTarget);
        FindHintInChildren(leftHandIKConstraint.transform, out leftHandIKHint);
        
        FindTargetInChildren(rightHandIKConstraint.transform, out rightHandIKTarget);
        FindHintInChildren(rightHandIKConstraint.transform, out rightHandIKHint);
    }

    private static void FindTargetInChildren(in Transform parent, out Transform target)
    {
        target = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name.Contains("Target"))
            {
                target = child;
                return;
            }
        }
    }
    private static void FindHintInChildren(in Transform parent, out Transform hint)
    {
        hint = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name.Contains("Hint"))
            {
                hint = child;
                return;
            }
        }
    }
}
