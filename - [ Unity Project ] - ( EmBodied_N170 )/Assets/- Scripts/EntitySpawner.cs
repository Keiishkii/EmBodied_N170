using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Transform _entityFolder;
    
    [Space]
    [SerializeField] private Vector3 _chairPosition;
    [SerializeField] private float _deskHeight;

    [Space] 
    [SerializeField] private List<EntityTypeAndPrefabReference> _dictionarySamples;
    private Dictionary<NPC_Enum, GameObject> _prefabDictionary;
    private Dictionary<NPC_Enum, GameObject> PrefabDictionary
    {
        get
        {
            if (ReferenceEquals(_prefabDictionary, null))
            {
                _prefabDictionary = new Dictionary<NPC_Enum, GameObject>();
                foreach (EntityTypeAndPrefabReference sample in _dictionarySamples)
                {
                    if (!_prefabDictionary.ContainsKey(sample.type))
                    {
                        _prefabDictionary.Add(sample.type, sample.prefab);
                    }
                    else
                    {
                        Debug.LogWarning($"Prefabs added using the same key to the dictionary. Key: {sample.type}");
                    }
                }
            }

            return _prefabDictionary;
        }
    }

    NPCData _npcOneData;
    NPCData _npcTwoData;
    
    
    
    
    
    public void SpawnNPCs(TestSegment testSegment, Transform npcLookAtTarget, out NPCController npc1Controller, out NPCController npc2Controller)
    {
        npc1Controller = null; npc2Controller = null;
        
        NPC_Enum npcType = testSegment.NPC;
        if (PrefabDictionary.ContainsKey(npcType))
        {
            GameObject NPC_1 = Instantiate(PrefabDictionary[npcType], _entityFolder);
            _npcOneData = new NPCData();
            _npcOneData.Populate(NPC_1);
            
            npc1Controller = _npcOneData.npcController;
            npc1Controller.SetLookAtTarget(npcLookAtTarget);
            
            
            GameObject NPC_2 = Instantiate(PrefabDictionary[npcType], _entityFolder);
            _npcTwoData = new NPCData();
            _npcTwoData.Populate(NPC_2);
            
            npc2Controller = _npcTwoData.npcController;
            npc2Controller.SetLookAtTarget(npcLookAtTarget);
            
            
            StartCoroutine(InitialiseNPC(_npcOneData, npcType, true));
            StartCoroutine(InitialiseNPC(_npcTwoData, npcType, false));
        }
        else
        {
            Debug.LogError($"Unable to load the NPC of type: {npcType}");
        }
    }

    public void ClearNPC()
    {
        Destroy(_npcOneData.gameObject);
        _npcOneData = null;
        
        Destroy(_npcTwoData.gameObject);
        _npcTwoData = null;
    }

    private IEnumerator InitialiseNPC(NPCData npc, NPC_Enum npcType, bool spawnInRoomA)
    {
        yield return new WaitForEndOfFrame();

        SetNPCTransformData(npc, npcType, spawnInRoomA);

        SetNPCArmIK(npc.npcIKReferences.leftHandIKConstraint, npc.npcIKReferences.leftHandIKHint, npc.npcIKReferences.leftHandIKTarget,
            npc.npcBoneReferences.leftForeArm, npc.npcBoneReferences.leftHand);
        
        SetNPCArmIK(npc.npcIKReferences.rightHandIKConstraint, npc.npcIKReferences.rightHandIKHint, npc.npcIKReferences.rightHandIKTarget, 
            npc.npcBoneReferences.rightForeArm, npc.npcBoneReferences.rightHand);

        
        SetNPCLegIK(npc.npcIKReferences.leftFootIKConstraint, npc.npcIKReferences.leftFootIKHint, npc.npcIKReferences.leftFootIKTarget,
            npc.npcBoneReferences.leftLeg, npc.npcBoneReferences.leftFoot, npc.npcBoneReferences.leftToeBase);
        
        SetNPCLegIK(npc.npcIKReferences.rightFootIKConstraint, npc.npcIKReferences.rightFootIKHint, npc.npcIKReferences.rightFootIKTarget,
            npc.npcBoneReferences.rightLeg, npc.npcBoneReferences.rightFoot, npc.npcBoneReferences.rightToeBase);
    }

    private void SetNPCTransformData(NPCData npc, NPC_Enum npcType, bool spawnInRoomA)
    {
        float NPCDefaultWaistHeight = PrefabDictionary[npcType].GetComponent<NPCBoneReferences>().root.position.y;
        float NPCSittingWaistHeight = npc.npcBoneReferences.root.position.y;
        
        Vector3 chairPosition = new Vector3((spawnInRoomA) ? (_chairPosition.x) : (_chairPosition.x * -1), _chairPosition.y, _chairPosition.z);
        Vector3 playerWaistOffset = new Vector3(0, NPCDefaultWaistHeight - NPCSittingWaistHeight, 0);
        
        Vector3 playerSpawnPosition = chairPosition - playerWaistOffset;
        Quaternion playerSpawnRotation = (spawnInRoomA) ? (Quaternion.Euler(0, -90, 0)) : (Quaternion.Euler(0, 90, 0));

        npc.gameObject.transform.SetPositionAndRotation(playerSpawnPosition, playerSpawnRotation);
    }

    private void SetNPCArmIK(TwoBoneIKConstraint IKConstraint, Transform hintAnchor, Transform targetAnchor, Transform elbowTransform, Transform handTransform)
    {
        Vector3 handPosition = handTransform.position;
        Vector3 targetPosition = new Vector3(handPosition.x, _deskHeight, handPosition.z);
        
        hintAnchor.SetPositionAndRotation(elbowTransform.position, elbowTransform.rotation);
        targetAnchor.SetPositionAndRotation(targetPosition, handTransform.rotation);
        
        IKConstraint.weight = 1.0f;
    }

    private void SetNPCLegIK(TwoBoneIKConstraint IKConstraint, Transform hintAnchor, Transform targetAnchor, Transform kneeTransform, Transform footTransform, Transform toeTransform)
    {
        Vector3 footPosition = footTransform.position, toePosition = toeTransform.position;
        float yOffset = footPosition.y - toePosition.y;
        
        Vector3 targetPosition = new Vector3(footPosition.x, yOffset, footPosition.z);
        
        hintAnchor.SetPositionAndRotation(kneeTransform.position, kneeTransform.rotation);
        targetAnchor.SetPositionAndRotation(targetPosition, footTransform.rotation);
        
        IKConstraint.weight = 1.0f;
    }
    
    
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 roomBChairPosition = new Vector3(_chairPosition.x * -1, _chairPosition.y, _chairPosition.z);
            Gizmos.DrawLine(_chairPosition, _chairPosition + new Vector3(0, 2, 0));
            Gizmos.DrawSphere(_chairPosition, 0.025f);
            Gizmos.DrawLine(roomBChairPosition, roomBChairPosition + new Vector3(0, 2, 0));
            Gizmos.DrawSphere(roomBChairPosition, 0.025f);
    }
}
