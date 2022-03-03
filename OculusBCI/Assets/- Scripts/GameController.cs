using System;
using System.Collections;
using System.Collections.Generic;
using DataCollection;
using Unity.Mathematics;
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



    [HideInInspector] public UnityEvent<Room_Enum> EnableColliders = new UnityEvent<Room_Enum>();
    [HideInInspector] public UnityEvent DisableColliders = new UnityEvent();


    private LSLMarkerOutputStream _lslMarkerOutputStream;
    private DataCollector _dataCollector;
        
    private EvaluationCanvasController _evaluationCanvasController;
    private RayInteractionController _rayInteractionController;
    private PlayerController _playerController;
    private EntitySpawner _entitySpawner;
    
    
    [SerializeField] private TextAsset _testDataStructureJSON;
    private TestDataStructure _testDataStructure;

    public GameState_Enum gameState { get; set; }

    private Room_Enum _chosenRoom;
    public Room_Enum ChosenRoom
    {
        set => _chosenRoom = value;
    }

    [Space(10)]
    [SerializeField] private Transform _playerCameraOffset;

    
    

    private void Awake()
    {
        _lslMarkerOutputStream = FindObjectOfType<LSLMarkerOutputStream>();
        _dataCollector = FindObjectOfType<DataCollector>();
        
        _evaluationCanvasController = FindObjectOfType<EvaluationCanvasController>();
        _rayInteractionController = FindObjectOfType<RayInteractionController>();
        _playerController = FindObjectOfType<PlayerController>();
        _entitySpawner = FindObjectOfType<EntitySpawner>();
    }

    [Serializable]
    private class TestOBJ
    {
        public List<string> stringList = new List<string>();
    }

    private void Start()
    {
        _testDataStructure = TestDataStructure.LoadJSONData(_testDataStructureJSON);
        StartCoroutine(GameFlow());
    }
    
    
    
    
    
    private IEnumerator GameFlow()
    {
        gameState = GameState_Enum.START;

        for (int phaseIndex = 0; phaseIndex < _testDataStructure.testSegments.Count; phaseIndex++)
        {   
            TestSegment testSegment = _testDataStructure.testSegments[phaseIndex];         
            
            //Spawn Entities
            yield return AwaitStateChange(GameState_Enum.START);
                _rayInteractionController.SetLaserVisibility(false);
                InitialiseSegmentSetup(testSegment, out NPCController npc1Controller, out NPCController npc2Controller);
            //-->>   
            
                
            // Lights In
            yield return AwaitStateChange(GameState_Enum.LIGHTS_ON);
                _playerCameraOffset.SetPositionAndRotation(Vector3.zero, quaternion.Euler(new Vector3(0, Mathf.PI / 2, 0)));
                gameState = GameState_Enum.AWAITING_PLAYER_APPROACH;
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
                gameState = GameState_Enum.AWAITING_PLAYER_DECISION;
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
                gameState = GameState_Enum.AWAITING_PLAYER_RETURN;
            //-->>   
            
            
            // Wait for player to return to (0, 0, 0)
            yield return AwaitStateChange(GameState_Enum.AWAITING_PLAYER_RETURN);
                StartCoroutine(_playerController.TestDistanceToCenter(_testDataStructure.distanceToTriggerReturnedToCentre));
            yield return AwaitStateChange(GameState_Enum.PLAYER_RETURNED);  
                gameState = GameState_Enum.LIGHTS_OFF; 
            //-->>    
            
            
            // Lights Out     
            yield return AwaitStateChange(GameState_Enum.LIGHTS_OFF);
                _playerCameraOffset.SetPositionAndRotation(new Vector3(0, -4, 0), quaternion.Euler(Vector3.zero));
                DestroyTestObjects();
                gameState = GameState_Enum.AWAITING_PLAYER_RESULTS; 
            //-->>   
            
            // Show the results UI and record the players inputs.
            yield return AwaitStateChange(GameState_Enum.AWAITING_PLAYER_RESULTS);
                _rayInteractionController.SetLaserVisibility(true);
                _evaluationCanvasController.CanvasEnabled(true);
                _evaluationCanvasController.LoadCanvasContent(_testDataStructure.testQuestionList);
            yield return AwaitStateChange(GameState_Enum.PLAYER_RESULTS_COLLECTED);
                gameState = GameState_Enum.CHECK_END; 
            //-->>   
            
            
            yield return AwaitStateChange(GameState_Enum.CHECK_END);   
                gameState = GameState_Enum.START;  
            //-->>      
        }
        
        Debug.Log("Test Complete");
        _dataCollector.PublishData(_testDataStructure.testQuestionList);
    }
    
    private IEnumerator AwaitStateChange(GameState_Enum state)
    {
        yield return new WaitUntil(() => gameState == state);      
        
        _lslMarkerOutputStream.WriteMarker($"State Change: {state}");
        _dataCollector.AddDataToContainer($"{state}", _playerController.GetHeadsetTransform());
        
        Debug.Log($"Phase State: <color=#00FFBB>{state}</color>");
    }
    
    private void InitialiseSegmentSetup(TestSegment testSegment, out NPCController npc1Controller, out NPCController npc2Controller)
    {
        _playerController.SpawnHeldItem(testSegment.heldItem);
        _entitySpawner.SpawnNPCs(testSegment, _playerController.GetHeadsetTransform(), out npc1Controller, out npc2Controller);
        
        gameState = GameState_Enum.LIGHTS_ON;
    }

    private void DestroyTestObjects()
    {
        _playerController.ClearHeldItem();
        _entitySpawner.ClearNPC();
    }
}
