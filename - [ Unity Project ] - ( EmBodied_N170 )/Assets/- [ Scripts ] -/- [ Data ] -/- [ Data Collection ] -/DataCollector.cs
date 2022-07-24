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

namespace Data {
    namespace DataCollection
    {
        public class DataCollector : MonoBehaviour
        {
            public readonly DataContainer dataContainer = new DataContainer();

            [SerializeField] private InputActionReference _leftHandActivation;
            [SerializeField] private InputActionReference _rightHandActivation;

            public BlockData CurrentBlockData => dataContainer.blockData[GameControllerStateMachine.blockIndex];
            public TrialData CurrentTrialData => CurrentBlockData.trialData[GameControllerStateMachine.trialIndex];



            private PlayerController _playerController;
            private PlayerController PlayerController => _playerController ?? (_playerController = FindObjectOfType<PlayerController>());
            
            private NetworkDataPublisher _networkDataPublisher;
            private NetworkDataPublisher NetworkDataPublisher => _networkDataPublisher ?? (_networkDataPublisher = FindObjectOfType<NetworkDataPublisher>());

            private GameControllerStateMachine _gameControllerStateMachine;
            private GameControllerStateMachine GameControllerStateMachine => _gameControllerStateMachine ?? (_gameControllerStateMachine = FindObjectOfType<GameControllerStateMachine>());


            private Transform CameraOffset => PlayerController.transform;
            private Transform CameraTransform => PlayerController.cameraTransform;
            private Transform RightHandTransform => PlayerController.rightHandTransform;
            private Transform LeftHandTransform => PlayerController.leftHandTransform;

            private IEnumerator _inputDataCollection;
            private bool _collectingData;








            private void Awake()
            {
                _leftHandActivation.action.performed += SampleInput;
                _rightHandActivation.action.performed += SampleInput;
            }

            private void OnDestroy()
            {
                _leftHandActivation.action.performed -= SampleInput;
                _rightHandActivation.action.performed -= SampleInput;
            }

            private void SampleInput(InputAction.CallbackContext callbackContext)
            {
                dataContainer.inputSamples.Add(new InputSampleData()
                {
                    inputType = callbackContext.action.name
                });
            }




            public void AddDataEventToContainer(in DataCollectionEvent_Interface dataCollectionEvent) { dataContainer.dataEvents.Add(dataCollectionEvent); }
            public void AddDataEventToContainer(in DataCollectionEvent_RecordMarker dataCollectionEvent) 
            {
                dataContainer.dataEvents.Add(dataCollectionEvent);
                
                #if PLATFORM_STANDALONE_WIN
                NetworkDataPublisher.PublishMarkerToNetwork(dataCollectionEvent.record);
                #endif
            }





            public void BeginTransformDataCollection()
            {
                _inputDataCollection = SampleTransformData();
                StartCoroutine(_inputDataCollection);
            }

            public void EndTransformDataCollection()
            {
                _collectingData = false;
                StopCoroutine(_inputDataCollection);
            }

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

                    yield return null;
                }
            }





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

            private static void WriteToCSV(string directory, string filename, DataContainer dataContainer)
            {

            }
        }
    }
}