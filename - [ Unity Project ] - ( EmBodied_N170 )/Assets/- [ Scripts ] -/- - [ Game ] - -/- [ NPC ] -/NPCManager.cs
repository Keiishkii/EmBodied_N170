using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCManager : MonoBehaviour
{
    private NPCData _npcOneData;
    public NPCData NpcOneData => _npcOneData;

    private NPCData _npcTwoData;
    public NPCData NpcTwoData => _npcTwoData;
    
    [SerializeField] private Transform _entityFolder;
    
    [Space]
    [SerializeField] private Vector3 _chairPosition;
    [SerializeField] private float _deskHeight;
    [SerializeField] private float _armClearance;





    public void CreateNPCs(GameObject roomA_NPCPrefab, GameObject roomB_NPCPrefab)
    {
        GameObject NPCOneGameObject = Instantiate(roomA_NPCPrefab, _entityFolder);
        _npcOneData = new NPCData();
        _npcOneData.Populate(NPCOneGameObject);
        
        InitialiseNPC(in _npcOneData, new Vector3(_chairPosition.x, _chairPosition.y, _chairPosition.z), Quaternion.Euler(0, -90, 0));
        
        GameObject NPCTwoGameObject = Instantiate(roomB_NPCPrefab, _entityFolder);
        _npcTwoData = new NPCData();
        _npcTwoData.Populate(NPCTwoGameObject);
        
        InitialiseNPC(in _npcTwoData, new Vector3(_chairPosition.x * -1, _chairPosition.y, _chairPosition.z), Quaternion.Euler(0, 90, 0));
    }

    public void DestroyNPC()
    {
        Destroy(_npcOneData.gameObject);
        _npcOneData = null;
        
        Destroy(_npcTwoData.gameObject);
        _npcTwoData = null;
    }

    
    
    private void InitialiseNPC(in NPCData npcData, in Vector3 position, in Quaternion rotation)
    {
        bool isRoomA = (position.x > 0);
        SetNPCTransformData(npcData, position, rotation);
        StartCoroutine(SetIK(npcData, isRoomA));
    }

    private void SetNPCTransformData(in NPCData npc, in Vector3 position, in Quaternion rotation)
    {
        float NPCDefaultWaistHeight = npc.npcBoneReferences.rootBasePosition.y;
        float NPCSittingWaistHeight = npc.npcBoneReferences.root.position.y;
        
        Vector3 chairPosition = position;
        Vector3 playerWaistOffset = new Vector3(0, NPCDefaultWaistHeight - NPCSittingWaistHeight, 0);
        
        Vector3 playerSpawnPosition = chairPosition - playerWaistOffset;
        Quaternion playerSpawnRotation = rotation;

        npc.gameObject.transform.SetPositionAndRotation(playerSpawnPosition, playerSpawnRotation);
    }


    private IEnumerator SetIK(NPCData npcData, bool isRoomA)
    {
        yield return new WaitForEndOfFrame();
        
        Debug.Log("NPC IK SET");
        
        SetNPCArmIK(npcData, isRoomA, false);
        SetNPCArmIK(npcData, isRoomA, true);
        
        SetNPCLegIK(npcData, isRoomA, false);
        SetNPCLegIK(npcData, isRoomA, true);
    }
    
    private void SetNPCArmIK(in NPCData npcData, in bool isRoomA, in bool isRight)
    {
        Transform handTransform, elbowTransform;
        Transform targetAnchor, hintAnchor;
        
        TwoBoneIKConstraint constraint;
        
        
        if (isRight)
        {
            handTransform = npcData.npcBoneReferences.rightHand;
            elbowTransform = npcData.npcBoneReferences.rightForeArm;
            
            targetAnchor = npcData.npcIKReferences.rightHandIKTarget;
            hintAnchor = npcData.npcIKReferences.rightHandIKHint;
            
            constraint = npcData.npcIKReferences.rightHandIKConstraint;
        }
        else
        {
            handTransform = npcData.npcBoneReferences.leftHand;
            elbowTransform = npcData.npcBoneReferences.leftForeArm;
            
            targetAnchor = npcData.npcIKReferences.leftHandIKTarget;
            hintAnchor = npcData.npcIKReferences.leftHandIKHint;
            
            constraint = npcData.npcIKReferences.leftHandIKConstraint;
        }
        
        Vector3 handTargetPosition = new Vector3()
        {
            x = handTransform.position.x,
            y = _deskHeight + _armClearance,
            z = handTransform.position.z
        };
        
        hintAnchor.SetPositionAndRotation(elbowTransform.position, elbowTransform.rotation);
        targetAnchor.SetPositionAndRotation(handTargetPosition, Quaternion.Euler(0, ((isRoomA) ? 180 : 0), 180));

        constraint.weight = 1.0f;
    }

    
    private void SetNPCLegIK(in NPCData npcData, in bool isRoomA, in bool isRight)
    {
        Transform toeTransform, footTransform, legTransform;
        Transform targetAnchor, hintAnchor;
        Transform root = npcData.npcBoneReferences.root;
        
        TwoBoneIKConstraint constraint;
        
        if (isRight)
        {
            toeTransform = npcData.npcBoneReferences.rightToeBase;
            footTransform = npcData.npcBoneReferences.rightFoot;
            legTransform = npcData.npcBoneReferences.rightLeg;
            
            targetAnchor = npcData.npcIKReferences.rightFootIKTarget;
            hintAnchor = npcData.npcIKReferences.rightFootIKHint;
            
            constraint = npcData.npcIKReferences.rightFootIKConstraint;
        }
        else
        {
            toeTransform = npcData.npcBoneReferences.leftToeBase;
            footTransform = npcData.npcBoneReferences.leftFoot;
            legTransform = npcData.npcBoneReferences.leftLeg;
            
            targetAnchor = npcData.npcIKReferences.leftFootIKTarget;
            hintAnchor = npcData.npcIKReferences.leftFootIKHint;
            
            constraint = npcData.npcIKReferences.leftFootIKConstraint;
        }

        Vector3 footTargetPosition = new Vector3()
        {
            x = footTransform.position.x,
            y = 0.0973f,
            z = footTransform.position.z
        };

        
        hintAnchor.SetPositionAndRotation(legTransform.position, legTransform.rotation);
        targetAnchor.SetPositionAndRotation(footTargetPosition, footTransform.rotation);
        
        constraint.weight = 1.0f;
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 roomBChairPosition = new Vector3(_chairPosition.x * -1, _chairPosition.y, _chairPosition.z);
        
        Gizmos.DrawLine(_chairPosition, _chairPosition + new Vector3(0, 2, 0));
        Gizmos.DrawSphere(_chairPosition, 0.025f);

        Gizmos.DrawLine(roomBChairPosition, roomBChairPosition + new Vector3(0, 2, 0));
        Gizmos.DrawSphere(roomBChairPosition, 0.025f);

        Vector3 roomBChairFlattened = new Vector3(roomBChairPosition.x, 0, roomBChairPosition.z);
        Gizmos.DrawLine(roomBChairFlattened + new Vector3(0, _deskHeight), roomBChairFlattened + new Vector3(0.75f, _deskHeight));
        Gizmos.DrawLine(roomBChairFlattened + new Vector3(0, _deskHeight + _armClearance), roomBChairFlattened + new Vector3(0.75f, _deskHeight + _armClearance));
    }
}
