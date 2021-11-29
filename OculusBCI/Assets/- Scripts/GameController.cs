using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get 
        {
            if (ReferenceEquals(_instance, null))
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }

    private PlayerController _playerController;
    private EntitySpawner _entitySpawner;
    
    [SerializeField] private TextAsset _testDataStructureJSON;
    private TestDataStructure _testDataStructure;
    private GameState_Enum _gameState;
    
    [Space]
    [SerializeField] private BoxCollider _roomADeskCollider;
    [SerializeField] private BoxCollider _roomBDeskCollider;



    private void Awake()
    {
        _roomADeskCollider.enabled = false;
        _roomBDeskCollider.enabled = false;
        
        _playerController = FindObjectOfType<PlayerController>();
        _entitySpawner = FindObjectOfType<EntitySpawner>();
    }

    private void Start()
    {
        _testDataStructure = (_testDataStructureJSON != null) ? 
            JsonUtility.FromJson<TestDataStructure>(_testDataStructureJSON.text) :
            TestDataStructure.DefaultFile;

        StartCoroutine(GameFlow());
    }

    
    
    private IEnumerator GameFlow()
    {
        _gameState = GameState_Enum.START;

        for (int phaseIndex = 0; phaseIndex < _testDataStructure.testSegments.Count; phaseIndex++)
        {
            bool phaseInRoomA = ((phaseIndex % 2) == 0);
            TestSegment testSegment = _testDataStructure.testSegments[phaseIndex];
            
            yield return new WaitUntil(() => _gameState == GameState_Enum.START); Debug.Log($"Phase State: {GameState_Enum.START}");
            InitialiseSegmentSetup(testSegment, phaseInRoomA, out NPCController npcController);
            
            yield return new WaitUntil(() => _gameState == GameState_Enum.AWAITING_PLAYER_APPROACH); Debug.Log($"Phase State: {GameState_Enum.AWAITING_PLAYER_APPROACH}");
            _playerController.StartCoroutine(_playerController.BeginDistanceTesting(npcController, _testDataStructure.distanceToTriggerLookUp));

            yield return new WaitUntil(() => _gameState == GameState_Enum.PLAYER_APPROACHED); Debug.Log($"Phase State: {GameState_Enum.PLAYER_APPROACHED}");
            ActivateDeskColliders(phaseInRoomA);
            
            yield return new WaitUntil(() => _gameState == GameState_Enum.AWAITING_PLAYER_INPUT); Debug.Log($"Phase State: {GameState_Enum.AWAITING_PLAYER_INPUT}");
            
            yield return new WaitUntil(() => _gameState == GameState_Enum.PLAYER_INPUT); Debug.Log($"Phase State: {GameState_Enum.PLAYER_INPUT}");
            DeactivateDeskColliders(phaseInRoomA);
            
            Debug.Log($"Phase {phaseIndex + 1} complete.");
            StartCoroutine(ReadyNextPhase());
        }
        
        Debug.Log("Test Complete");
    }

    private IEnumerator ReadyNextPhase()
    {
        yield return new WaitForSeconds(5.0f);
        DestroyTestObjects();
        
        _gameState = GameState_Enum.START;
    }
    
    private void InitialiseSegmentSetup(TestSegment testSegment, bool phaseInRoomA, out NPCController npcController)
    {
        _playerController.SpawnHeldItem(testSegment.heldItem);
        _entitySpawner.SpawnNPC(testSegment, phaseInRoomA, out npcController);
        npcController.SetLookAtTarget(_playerController.GetHeadsetTransform());
        
        _gameState = GameState_Enum.AWAITING_PLAYER_APPROACH;
    }

    private void DestroyTestObjects()
    {
        _playerController.ClearHeldItem();
        _entitySpawner.ClearNPC();
    }
    
    private void ActivateDeskColliders(bool phaseInRoomA)
    {
        if (phaseInRoomA)
        {
            _roomADeskCollider.enabled = true;
        }
        else
        {
            _roomBDeskCollider.enabled = true;
        }

        _gameState = GameState_Enum.AWAITING_PLAYER_INPUT;
    }
    
    private void DeactivateDeskColliders(bool phaseInRoomA)
    {
        if (phaseInRoomA)
        {
            _roomADeskCollider.enabled = false;
        }
        else
        {
            _roomBDeskCollider.enabled = false;
        }
    }

    public void SetState(GameState_Enum state)
    {
        _gameState = state;
    }
}
