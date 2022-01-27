using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _headsetTransform;
    [SerializeField] private Transform _leftHandTransform, _rightHandTransform;

    [Space] 
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 eulerRotationOffset;
    
    [Space]
    [SerializeField] private List<ItemTypeAndPrefabReference> _dictionarySamples;
    private Dictionary<Item_Enum, GameObject> _prefabDictionary;
    private Dictionary<Item_Enum, GameObject> PrefabDictionary
    {
        get
        {
            if (ReferenceEquals(_prefabDictionary, null))
            {
                _prefabDictionary = new Dictionary<Item_Enum, GameObject>();
                foreach (ItemTypeAndPrefabReference sample in _dictionarySamples)
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

    private GameObject _heldItem;
    
    
    
    
    
    public IEnumerator TestDistanceForPlayerApproach(NPCController npcController, float distance)
    {
        Transform npcHeadTransform = npcController.GetHeadTransform();
        Vector3 position, npcFacePosition;
        
        do
        {
            position = _headsetTransform.position;
            position = new Vector3(position.x, 0, position.z);
            npcFacePosition = npcHeadTransform.position;
            npcFacePosition = new Vector3(npcFacePosition.x, 0, npcFacePosition.z);

            yield return null;
        } while (Vector3.SqrMagnitude(position - npcFacePosition) > Mathf.Pow(distance, 2));

        npcController.LookAt(true);

        GameController.Instance.ChosenRoom = (transform.position.x > 0) ? Room_Enum.ROOM_A : Room_Enum.ROOM_B;
        GameController.Instance.GameState = GameState_Enum.PLAYER_APPROACHED;
    }

    public IEnumerator TestDistanceForWalkingAway(NPCController npcController, float distance)
    {
        Transform npcHeadTransform = npcController.GetHeadTransform();
        Vector3 position, npcFacePosition;
        
        do
        {
            position = _headsetTransform.position;
            position = new Vector3(position.x, 0, position.z);
            npcFacePosition = npcHeadTransform.position;
            npcFacePosition = new Vector3(npcFacePosition.x, 0, npcFacePosition.z);

            yield return null;
        } while (Vector3.SqrMagnitude(position - npcFacePosition) < Mathf.Pow(distance, 2));
        
        npcController.LookAt(false);
        
        GameController.Instance.GameState = GameState_Enum.PLAYER_DECISION;
    }

    public IEnumerator TestDistanceToCenter(float distance)
    {
        Vector3 position;
        
        do
        {
            position = _headsetTransform.position;
            position = new Vector3(position.x, 0, position.z);
            
            yield return null;
        } while (Vector3.SqrMagnitude(position) > Mathf.Pow(distance, 2));
        
        GameController.Instance.GameState = GameState_Enum.PLAYER_RETURNED;
    }
    
    public void SpawnHeldItem(Item_Enum itemType)
    {
        if (PrefabDictionary.ContainsKey(itemType))
        {
            _heldItem = Instantiate(PrefabDictionary[itemType], _rightHandTransform);
            _heldItem.transform.localPosition = positionOffset;
            _heldItem.transform.rotation = Quaternion.Euler(eulerRotationOffset);
        }
        else
        {
            Debug.LogError($"Unable to load the item of type: {itemType}");
        }
    }

    public void ClearHeldItem()
    {
        Destroy(_heldItem);
    }

    public Transform GetHeadsetTransform()
    {
        return _headsetTransform;
    }
}
