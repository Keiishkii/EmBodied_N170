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

    private GameObject _spawnedNPC;
    private NPCController _npcController;
    private NPCBoneReferences _npcBoneReferences;
    private NPCIKReferences _npcIKReferences;
    
    
    
    
    
    public void SpawnNPC(TestSegment testSegment, bool phaseInRoomA, out NPCController npcController)
    {
        npcController = null;
        
        NPC_Enum npcType = testSegment.NPC;
        if (PrefabDictionary.ContainsKey(npcType))
        {
            _spawnedNPC = Instantiate(PrefabDictionary[npcType], _entityFolder);
            _npcController = _spawnedNPC.GetComponent<NPCController>();
            _npcBoneReferences = _spawnedNPC.GetComponent<NPCBoneReferences>();
            _npcIKReferences = _spawnedNPC.GetComponent<NPCIKReferences>();
            
            npcController = _npcController;
            
            StartCoroutine(InitialiseNPC(npcType, phaseInRoomA));
        }
        else
        {
            Debug.LogError($"Unable to load the NPC of type: {npcType}");
        }
    }

    public void ClearNPC()
    {
        Destroy(_spawnedNPC);
    }

    private IEnumerator InitialiseNPC(NPC_Enum NPCType, bool spawnInRoomA)
    {
        yield return new WaitForEndOfFrame();

        SetNPCTransformData(NPCType, spawnInRoomA);

        SetNPCArmIK(_npcIKReferences.leftHandIKConstraint, _npcIKReferences.leftHandIKHint, _npcIKReferences.leftHandIKTarget,
            _npcBoneReferences.leftForeArm, _npcBoneReferences.leftHand);
        
        SetNPCArmIK(_npcIKReferences.rightHandIKConstraint, _npcIKReferences.rightHandIKHint, _npcIKReferences.rightHandIKTarget, 
            _npcBoneReferences.rightForeArm, _npcBoneReferences.rightHand);

        
        SetNPCLegIK(_npcIKReferences.leftFootIKConstraint, _npcIKReferences.leftFootIKHint, _npcIKReferences.leftFootIKTarget,
            _npcBoneReferences.leftLeg, _npcBoneReferences.leftFoot, _npcBoneReferences.leftToeBase);
        
        SetNPCLegIK(_npcIKReferences.rightFootIKConstraint, _npcIKReferences.rightFootIKHint, _npcIKReferences.rightFootIKTarget,
            _npcBoneReferences.rightLeg, _npcBoneReferences.rightFoot, _npcBoneReferences.rightToeBase);
    }

    private void SetNPCTransformData(NPC_Enum NPCType, bool spawnInRoomA)
    {
        float NPCDefaultWaistHeight = PrefabDictionary[NPCType].GetComponent<NPCBoneReferences>().root.position.y;
        float NPCSittingWaistHeight = _npcBoneReferences.root.position.y;
        
        Vector3 chairPosition = new Vector3((spawnInRoomA) ? (_chairPosition.x) : (_chairPosition.x * -1), _chairPosition.y, _chairPosition.z);
        Vector3 playerWaistOffset = new Vector3(0, NPCDefaultWaistHeight - NPCSittingWaistHeight, 0);
        
        Vector3 playerSpawnPosition = chairPosition - playerWaistOffset;
        Quaternion playerSpawnRotation = (spawnInRoomA) ? (Quaternion.Euler(0, -90, 0)) : (Quaternion.Euler(0, 90, 0));

        _spawnedNPC.transform.SetPositionAndRotation(playerSpawnPosition, playerSpawnRotation);
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
