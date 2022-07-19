using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBoneReferences : MonoBehaviour
{
    public Transform root;
    
    [Space(10)] 
    public Transform head;
    
    public Transform leftEye;
    public Transform rightEye;

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


    [ContextMenu("Auto Complete 'Bone References'")]
    public void AutoComplete()
    {
        Transform transform = this.transform;
        
        if (!SearchHierarchyForGameObject(transform, "Bip01", ref root)) 
            Debug.LogError("Could not find the transform: Root");
        
        
        if (!SearchHierarchyForGameObject(transform, "Bip01 Head", ref head)) 
            Debug.LogError("Could not find the transform: Head");
        
        if (!SearchHierarchyForGameObject(transform, "Bip01 LEye", ref leftEye)) 
            Debug.LogError("Could not find the transform: Left Eye");
        if (!SearchHierarchyForGameObject(transform, "Bip01 REye", ref rightEye)) 
            Debug.LogError("Could not find the transform: Right Eye");
        
        
        if (!SearchHierarchyForGameObject(transform, "Bip01 L Hand", ref leftHand)) 
            Debug.LogError("Could not find the transform: Left Hand");
        if (!SearchHierarchyForGameObject(transform, "Bip01 L Forearm", ref leftForeArm)) 
            Debug.LogError("Could not find the transform: Left Forearm");
        
        if (!SearchHierarchyForGameObject(transform, "Bip01 R Hand", ref rightHand)) 
            Debug.LogError("Could not find the transform: Right Hand");
        if (!SearchHierarchyForGameObject(transform, "Bip01 R Forearm", ref rightForeArm)) 
            Debug.LogError("Could not find the transform: Right Forearm");
        
        
        if (!SearchHierarchyForGameObject(transform, "Bip01 L Toe0", ref leftToeBase)) 
            Debug.LogError("Could not find the transform: Left Toe");
        if (!SearchHierarchyForGameObject(transform, "Bip01 L Foot", ref leftFoot)) 
            Debug.LogError("Could not find the transform: Left Foot");
        if (!SearchHierarchyForGameObject(transform, "Bip01 L Calf", ref leftLeg)) 
            Debug.LogError("Could not find the transform: Left Calf");
        
        if (!SearchHierarchyForGameObject(transform, "Bip01 R Toe0", ref rightToeBase)) 
            Debug.LogError("Could not find the transform: Right Toe");
        if (!SearchHierarchyForGameObject(transform, "Bip01 R Foot", ref rightFoot)) 
            Debug.LogError("Could not find the transform: Right Foot");
        if (!SearchHierarchyForGameObject(transform, "Bip01 R Calf", ref rightLeg)) 
            Debug.LogError("Could not find the transform: Right Calf");
    }

    private static bool SearchHierarchyForGameObject(in Transform parent, in string name, ref Transform found)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (!child.name.Equals(name))
            {
                if (SearchHierarchyForGameObject(child, name, ref found))
                {
                    return true;
                }
            }
            else
            {
                found = child;
                return true;
            }
        }

        return false;
    }
}
