using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItemProperties : MonoBehaviour
{
    public Vector3 leftHandHoldingPositionOffset;
    public Vector3 leftHandHoldingEulerRotationOffset;
    
    [Space (5)]
    public Vector3 rightHandHoldingPositionOffset;
    public Vector3 rightHandHoldingEulerRotationOffset;
    
    [Space (10)]
    public Vector3 placementPositionOffset;
    public Vector3 placementEulerRotationOffset;
}
