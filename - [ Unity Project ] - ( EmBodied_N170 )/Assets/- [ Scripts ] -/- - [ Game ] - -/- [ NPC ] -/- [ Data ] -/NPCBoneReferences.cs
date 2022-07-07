using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBoneReferences : MonoBehaviour
{
    public Transform root;
    
    [Space(10)] 
    public Transform head;
    public Transform headTop;

    [Space(10)] 
    public Transform leftHand;
    public Transform leftForeArm;

    [Space(10)] 
    public Transform rightHand;
    public Transform rightForeArm;

    [Space(10)]
    public Transform leftToeBase;
    public Transform leftFoot;
    public Transform leftLeg;

    [Space(10)] 
    public Transform rightToeBase;
    public Transform rightFoot;
    public Transform rightLeg;
    
    [HideInInspector] public Vector3 
        headBasePosition,
        rootBasePosition,
        
        leftHandBasePosition,
        leftForeArmBasePosition,
        rightHandBasePosition,
        rightForeArmBasePosition,
        
        leftToeBaseBasePosition,
        leftFootBasePosition,
        leftLegBasePosition,
        rightToeBaseBasePosition,
        rightFootBasePosition,
        rightLegBasePosition;
    
    
    
    
    
    private void Awake()
    {
        headBasePosition = head.position;
        rootBasePosition = root.position;

        leftHandBasePosition = leftHand.position;
        leftForeArmBasePosition = leftForeArm.position;
        rightHandBasePosition = rightHand.position;
        rightForeArmBasePosition = rightForeArm.position;

        leftToeBaseBasePosition = leftToeBase.position;
        leftFootBasePosition = leftFoot.position;
        leftLegBasePosition = leftLeg.position;
        rightToeBaseBasePosition = rightToeBase.position;
        rightFootBasePosition = rightFoot.position;
        rightLegBasePosition = rightLeg.position;
    }
}
