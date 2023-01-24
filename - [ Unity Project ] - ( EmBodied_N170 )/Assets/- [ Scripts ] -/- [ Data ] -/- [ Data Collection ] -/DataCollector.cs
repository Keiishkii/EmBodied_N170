using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// The DataCollector class is the interface used for collecting data of a session.
        /// This is used to capture and then write the movements, actions and states of the player and the experiment to a JSON file. 
        /// </summary>
        public class DataCollector : MonoBehaviour
        {
            // The container class for storing all data within the session, this file is what will be saved to the JSON format.
            public readonly DataContainer dataContainer = new DataContainer();

            [SerializeField] private InputActionReference _leftHandActivation;
            [SerializeField] private InputActionReference _rightHandActivation;

            // A reference to the active Block currently being performed.
            public BlockData CurrentBlockData => dataContainer.blockData[GameControllerStateMachine.blockIndex];
            // A reference to the active Trial currently being performed
            public TrialData CurrentTrialData => CurrentBlockData.trialData[GameControllerStateMachine.trialIndex];


            // A reference to the player controller.
            private PlayerController _playerController;
            private PlayerController PlayerController => _playerController ? _playerController : (_playerController = FindObjectOfType<PlayerController>());
            
            // A reference to the networks data publisher.
            private NetworkDataPublisher _networkDataPublisher;
            private NetworkDataPublisher NetworkDataPublisher => _networkDataPublisher ? _networkDataPublisher : (_networkDataPublisher = FindObjectOfType<NetworkDataPublisher>());

            // A reference to the game controller.
            private GameControllerStateMachine _gameControllerStateMachine;
            private GameControllerStateMachine GameControllerStateMachine => _gameControllerStateMachine ? _gameControllerStateMachine : (_gameControllerStateMachine = FindObjectOfType<GameControllerStateMachine>());


            // Transform references, used to track the positions of player movements within the game.
            private Transform CameraOffset => PlayerController.transform;
            private Transform CameraTransform => PlayerController.cameraTransform;
            private Transform RightHandTransform => PlayerController.rightHandTransform;
            private Transform LeftHandTransform => PlayerController.leftHandTransform;

            // The coroutine responsible for recording the players transform data.
            private IEnumerator _inputDataCollection;
            private bool _collectingData;








            private void Awake()
            {
                // Subscription to key input events to track player inputs.
                _leftHandActivation.action.performed += SampleInput;
                _rightHandActivation.action.performed += SampleInput;
            }

            private void OnDestroy()
            {
                // Unsubscription to key input events.
                _leftHandActivation.action.performed -= SampleInput;
                _rightHandActivation.action.performed -= SampleInput;
            }

            // Adds the sampled input and its key to the data containers list of input samples.
            private void SampleInput(InputAction.CallbackContext callbackContext)
            {
                dataContainer.inputSamples.Add(new InputSampleData()
                {
                    inputType = callbackContext.action.name
                });
            }




            // Adds and event to the data container, these events describe the state of the experiment and what is going on during it.
            public void AddDataEventToContainer(in DataCollectionEvent_Interface dataCollectionEvent) { dataContainer.dataEvents.Add(dataCollectionEvent); }
            public void AddDataEventToContainer(in DataCollectionEvent_RecordMarker dataCollectionEvent) 
            {
                #if (PLATFORM_STANDALONE_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
                    NetworkDataPublisher.PublishMarkerToNetwork(dataCollectionEvent.record);
                #endif

                Debug.Log($"<color=#44FFAA>Marker</color>: {dataCollectionEvent.record}");
                dataContainer.dataEvents.Add(dataCollectionEvent);
            }




            // Beings a new coroutine for sampling transform data.
            public void BeginTransformDataCollection()
            {
                _inputDataCollection = SampleTransformData();
                StartCoroutine(_inputDataCollection);
            }

            // Stops the current coroutine from sampling transform data.
            public void EndTransformDataCollection()
            {
                _collectingData = false;
                StopCoroutine(_inputDataCollection);
            }

            // Writes the transform data from the head, camera and both hands to the data containers transform sample list. 
            // This is performed each frame.
            private IEnumerator SampleTransformData()
            {
                _collectingData = true;
                while (_collectingData)
                {
                    dataContainer.transformSamples.Add(new TransformSampleData()
                    {
                        time = Time.realtimeSinceStartup,

                        SetCameraOffsetTransform = CameraOffset,
                        SetHeadTransform = CameraTransform,
                        SetLeftHandTransform = RightHandTransform,
                        SetRightHandTransform = LeftHandTransform
                    });

                    // Yields until the next frame.
                    yield return null;
                }
            }




            // Experiments data write function, used to write the recorded data (using a separate thread) to a file.
            public void WriteData()
            {
                string dateTime = $"{DateTime.Now:U}";
                dateTime = dateTime.Replace(':', '-');

                string directory = $"{Application.persistentDataPath}/Data/";
                string filename = $"DataContainer - {dateTime}";

                Thread writeToJson = new Thread(() => WriteToJSON(directory, filename, dataContainer));
                Thread writeToCSV = new Thread(() => WriteToCSV(directory, filename, dataContainer));

                writeToJson.Start();
                writeToCSV.Start();
            }

            // Write function for converting and the saving the data container to a JSON file.
            private static void WriteToJSON(string directory, string filename, DataContainer dataContainer)
            {
                string jsonData = JsonConvert.SerializeObject(dataContainer, Formatting.Indented);

                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                try
                {
                    File.WriteAllText($"{directory}{filename}", jsonData);
                    Debug.Log($"Entered State: <color=#4F4>Data Written To:</color> {directory}{filename}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            // Unused conversion class for CSV data
            private static void WriteToCSV(string directory, string filename, DataContainer dataContainer)
            {

            }
        }
    }
}