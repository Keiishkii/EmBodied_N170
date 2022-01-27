using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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



    public UnityEvent<Room_Enum> EnableColliders = new UnityEvent<Room_Enum>();
    public UnityEvent DisableColliders = new UnityEvent();
    
        
    
    private EvaluationCanvasController _evaluationCanvasController;
    private PlayerController _playerController;
    private LightController _lightController;
    private EntitySpawner _entitySpawner;
    
    
    [SerializeField] private TextAsset _testDataStructureJSON;
    private TestDataStructure _testDataStructure;
    
    private GameState_Enum _gameState;
    public GameState_Enum GameState
    {
        get => _gameState;
        set
        {
            if (value == _gameState + 1)
            {
                _gameState = value;
            }
        }
    }

    private Room_Enum _chosenRoom;
    public Room_Enum ChosenRoom
    {
        set => _chosenRoom = value;
    }


    
    

    private void Awake()
    {
        _evaluationCanvasController = FindObjectOfType<EvaluationCanvasController>();
        _playerController = FindObjectOfType<PlayerController>();
        _lightController = FindObjectOfType<LightController>();
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
            TestSegment testSegment = _testDataStructure.testSegments[phaseIndex];         
            
            //Spawn Entities
            yield return AwaitStateChange(GameState_Enum.START);
                InitialiseSegmentSetup(testSegment, out NPCController npc1Controller, out NPCController npc2Controller);
            //-->>   
            
                
            //Begin Lights Fade In
            yield return AwaitStateChange(GameState_Enum.FADE_LIGHTS_IN);   
                _lightController.StartFadeLightsIn();
            yield return AwaitStateChange(GameState_Enum.LIGHTS_ON);
                GameState = GameState_Enum.AWAITING_PLAYER_APPROACH;
            //-->>   
            
            
            // Wait for player to get close enough to the NPC
            yield return AwaitStateChange(GameState_Enum.AWAITING_PLAYER_APPROACH);
                IEnumerator playerApproachNPC1 = _playerController.TestDistanceForPlayerApproach(npc1Controller, _testDataStructure.distanceToTriggerNPCLookUp);
                StartCoroutine(playerApproachNPC1);
                IEnumerator playerApproachNPC2 = _playerController.TestDistanceForPlayerApproach(npc2Controller, _testDataStructure.distanceToTriggerNPCLookUp);
                StartCoroutine(playerApproachNPC2);
            yield return AwaitStateChange(GameState_Enum.PLAYER_APPROACHED); 
                StopCoroutine(playerApproachNPC1);
                StopCoroutine(playerApproachNPC2);
                GameState = GameState_Enum.AWAITING_PLAYER_DECISION;
            //-->>   
            
            
            // Wait for player to either walk away, or place there held item on the desk.
            yield return AwaitStateChange(GameState_Enum.AWAITING_PLAYER_DECISION);
                IEnumerator playerLeave = _playerController.TestDistanceForWalkingAway(((Room_Enum.ROOM_A == _chosenRoom) ? npc1Controller : npc2Controller), _testDataStructure.distanceToTriggerPlayerLeave);
                StartCoroutine(playerLeave);
                EnableColliders.Invoke(_chosenRoom);
            yield return AwaitStateChange(GameState_Enum.PLAYER_DECISION);
                ((Room_Enum.ROOM_A == _chosenRoom) ? npc1Controller : npc2Controller).LookAt(false);
                StopCoroutine(playerLeave);
                DisableColliders.Invoke();
                GameState = GameState_Enum.AWAITING_PLAYER_RETURN;
            //-->>   
            
            
            // Wait for player to return to (0, 0, 0)
            yield return AwaitStateChange(GameState_Enum.AWAITING_PLAYER_RETURN);
                StartCoroutine(_playerController.TestDistanceToCenter(_testDataStructure.distanceToTriggerReturnedToCentre));
            yield return AwaitStateChange(GameState_Enum.PLAYER_RETURNED);  
                GameState = GameState_Enum.FADE_LIGHTS_OUT; 
            //-->>    
            
            
            //Begin Lights Fade Out     
            yield return AwaitStateChange(GameState_Enum.FADE_LIGHTS_OUT);    
                _lightController.StartFadeLightsOut();
            yield return AwaitStateChange(GameState_Enum.LIGHTS_OFF);
                DestroyTestObjects();
                GameState = GameState_Enum.AWAITING_PLAYER_RESULTS; 
            //-->>   
            
            // Show the results UI and record the players inputs.
            yield return AwaitStateChange(GameState_Enum.AWAITING_PLAYER_RESULTS);
                _evaluationCanvasController.CanvasEnabled(true);
                _evaluationCanvasController.LoadCanvasContent(_testDataStructure.testQuestions);
            yield return AwaitStateChange(GameState_Enum.PLAYER_RESULTS_COLLECTED);
            //-->>   
            
            
            yield return AwaitStateChange(GameState_Enum.CHECK_END);    
            //-->>      
        }
        
        Debug.Log("Test Complete");
    }
    
    private IEnumerator AwaitStateChange(GameState_Enum state)
    {
        yield return new WaitUntil(() => _gameState == state);                      
        
        Debug.Log($"Phase State: <color=#00FFBB>{state}</color>");
    }
    
    private void InitialiseSegmentSetup(TestSegment testSegment, out NPCController npc1Controller, out NPCController npc2Controller)
    {
        _playerController.SpawnHeldItem(testSegment.heldItem);
        _entitySpawner.SpawnNPCs(testSegment, _playerController.GetHeadsetTransform(), out npc1Controller, out npc2Controller);
        
        GameState = GameState_Enum.FADE_LIGHTS_IN;
    }

    private void DestroyTestObjects()
    {
        _playerController.ClearHeldItem();
        _entitySpawner.ClearNPC();
    }
    
    private IEnumerator ReadyNextPhase()
    {
        yield return new WaitForSeconds(5.0f);
        
        _gameState = GameState_Enum.START;
    }
}
