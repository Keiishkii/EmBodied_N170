using System;
using System.Collections;
using Enums;
using Questionnaire;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace StateMachine
{
    public class State_TrialStart : State_Interface
    {
        public static readonly UnityEvent<GameControllerStateMachine> StartTrial = new UnityEvent<GameControllerStateMachine>();
        
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());

        private HandAnimationController _handAnimationController;
        private HandAnimationController HandAnimationController => _handAnimationController ?? (_handAnimationController = GameObject.FindObjectOfType<HandAnimationController>());

        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());

        private LookAtCursor _lookAtCursor;
        private LookAtCursor LookAtCursor => _lookAtCursor ?? (_lookAtCursor = GameObject.FindObjectOfType<LookAtCursor>());

        private float _radius = 0.25f;
        
        private IEnumerator _alignmentCheckCoroutine;
        private IEnumerator _trialStartCoroutine;
        
        
        


        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex, trialIndex = stateMachine.trialIndex;
            
            Debug.Log($"Entered State: <color=#FFF>Trial Start: {trialIndex}</color>");


            MainCanvas.LookTarget.SetActive(false);
            MainCanvas.ReadyPanelVisible = true;
            
            stateMachine.currentTrial = stateMachine.currentBlock.trials[trialIndex];
            Data.Input.Trial currentTrial = stateMachine.currentTrial;
            DataCollector.CurrentBlockData.trialData.Add(new Data.DataCollection.TrialData());

            RayInteractionController.RayVisibility = false;
            LookAtCursor.CursorActive = true;

            Handedness handedness = stateMachine.SessionFormatObject.participantHandedness;
            switch (handedness)
            {
                case Handedness.Right:
                {
                    HandAnimationController.RightHandState = HandAnimationState.Holding;
                    HandAnimationController.LeftHandState = HandAnimationState.Default;
                } break;
                case Handedness.Left: 
                {
                    HandAnimationController.RightHandState = HandAnimationState.Default;
                    HandAnimationController.LeftHandState = HandAnimationState.Holding;
                } break;
            }
            
            PlayerController.CreateHeldItem(currentTrial.heldObject, handedness);
            NPCManager.CreateNPCs(currentTrial.roomA_NPCAvatar, currentTrial.roomB_NPCAvatar);

            NPCDataCollectionData characterDataA = currentTrial.roomA_NPCAvatar.GetComponent<NPCDataCollectionData>();
            NPCDataCollectionData characterDataB = currentTrial.roomB_NPCAvatar.GetComponent<NPCDataCollectionData>();

            DataCollector.CurrentTrialData.roomACharacterName = currentTrial.roomA_NPCAvatar.name;
            DataCollector.CurrentTrialData.roomACharacterID = characterDataA.characterID;

            DataCollector.CurrentTrialData.roomBCharacterName = currentTrial.roomB_NPCAvatar.name;
            DataCollector.CurrentTrialData.roomBCharacterID = characterDataB.characterID;

            _alignmentCheckCoroutine = LightsOnState(stateMachine);
            
            StartTrial.AddListener(StartTrialCoroutine);
            
            stateMachine.StartCoroutine(_alignmentCheckCoroutine);
        }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            stateMachine.StopCoroutine(_alignmentCheckCoroutine);
            StartTrial.RemoveListener(StartTrialCoroutine);
            
            NPCDataCollectionData characterDataA = stateMachine.currentTrial.roomA_NPCAvatar.GetComponent<NPCDataCollectionData>();
            NPCDataCollectionData characterDataB = stateMachine.currentTrial.roomB_NPCAvatar.GetComponent<NPCDataCollectionData>();

            int trialID = (stateMachine.currentBlock.targetRoom == Room.RoomA) ?
                    ((!characterDataA.isStatue) ? 1 : 3) :
                    ((!characterDataB.isStatue) ? 2 : 4);

            DataCollector.AddDataEventToContainer(new Data.DataCollection.DataCollectionEvent_RecordMarker()
            {
                record = $"{trialID}"
            });

            MainCanvas.ReadyPanelVisible = false;
            LookAtCursor.CursorActive = false;
        }
        
        private bool GetLookTargetPosition(out Vector3 position)
        {
            position = new Vector3();
            
            Transform
                cameraOffsetTransform = PlayerController.transform,
                playerHeadTransform = PlayerController.cameraTransform,
                
                npcLeftEye = NPCManager.NpcOneData.npcBoneReferences.leftEye,
                npcRightEye = NPCManager.NpcOneData.npcBoneReferences.leftEye,
                
                mainCanvasTransform = MainCanvas.transform;

            Vector3
                positionOfOffset = cameraOffsetTransform.position,
                positionOfHead = playerHeadTransform.position,
                
                positionOfNPCLeftEye = npcLeftEye.position,
                positionOfNPCRightEye = npcRightEye.position,
                
                mainCanvasPosition = mainCanvasTransform.position;

            Vector3 correctedPlayerHeadPosition = positionOfHead - positionOfOffset;
            Vector3 correctedNPCHeadPosition = (positionOfNPCLeftEye + positionOfNPCRightEye) / 2.0f;
            
            
            
            Plane canvasPlane = new Plane(mainCanvasTransform.forward * -1, mainCanvasPosition);
            Ray ray = new Ray(positionOfHead, Vector3.Normalize(correctedNPCHeadPosition - correctedPlayerHeadPosition));

            if (canvasPlane.Raycast(ray, out float distance))
            {
                position = ray.GetPoint(distance);
                return true;
            }
            
            return false;
        }

        private IEnumerator LightsOnState(GameControllerStateMachine stateMachine)
        {
            Transform playerHeadTransform = PlayerController.cameraTransform;
            Transform lookTargetTransform = MainCanvas.LookTarget.transform;

            yield return null;
            MainCanvas.LookTarget.SetActive(true);

            float focusDuration = 0.5f, timeElapsed = 0;
            while (true)
            {
                if (GetLookTargetPosition(out Vector3 position))
                {
                    lookTargetTransform.position = position;
                }

                if (LookDirectionCheck(ref playerHeadTransform, ref lookTargetTransform) && BoundsCheck(ref playerHeadTransform))
                {
                    timeElapsed += Time.deltaTime;
                    Debug.Log($"Focus: {Mathf.InverseLerp(0, focusDuration, timeElapsed) * 100}%");

                    if (timeElapsed > focusDuration) break;
                }
                else timeElapsed = 0;

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            stateMachine.CurrentState = stateMachine.LightsOn;
        }

        private bool LookDirectionCheck(ref Transform playerHeadTransform, ref Transform lookTargetTransform)
        {
            Vector3 
                targetPosition = lookTargetTransform.position, 
                playerHeadPosition = playerHeadTransform.position;

            float result = Vector3.Dot(
                PlayerController.cameraTransform.forward,
                Vector3.Normalize(targetPosition - playerHeadPosition));

            float cos = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * 1.5f)); // Degrees to dot result
            return (result > cos);
        }

        private bool BoundsCheck(ref Transform playerHeadTransform)
        {
            Vector3 position = playerHeadTransform.position;
            Vector3 flattenedPosition = new Vector3(position.x, 0, position.z);

            return (Vector3.SqrMagnitude(flattenedPosition) < Mathf.Pow(_radius, 2));
        }

        private void StartTrialCoroutine(GameControllerStateMachine stateMachine)
        {
            stateMachine.CurrentState = stateMachine.LightsOn;
        }






        public override void OnDrawGizmos(GameControllerStateMachine stateMachine)
        {
            Transform
                cameraOffsetTransform = PlayerController.transform,
                playerHeadTransform = PlayerController.cameraTransform,
                npcHeadTransform = NPCManager.NpcOneData.npcBoneReferences.head,
                mainCanvasTransform = MainCanvas.transform;

            Vector3
                positionOfOffset = cameraOffsetTransform.position,
                positionOfHead = playerHeadTransform.position,
                positionOfNPCHead = npcHeadTransform.position,
                mainCanvasPosition = mainCanvasTransform.position;

            Vector3 correctedPlayerHeadPosition = positionOfHead - positionOfOffset;
            
            
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(mainCanvasPosition, mainCanvasPosition + mainCanvasTransform.forward * -1);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(positionOfHead, positionOfHead + playerHeadTransform.forward);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(correctedPlayerHeadPosition, correctedPlayerHeadPosition + Vector3.Normalize(positionOfNPCHead - correctedPlayerHeadPosition));
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(positionOfHead, positionOfHead + Vector3.Normalize(positionOfNPCHead - correctedPlayerHeadPosition));
            
            
            Plane canvasPlane = new Plane(mainCanvasTransform.forward * -1, mainCanvasPosition);
            Ray ray = new Ray(positionOfHead, Vector3.Normalize(positionOfNPCHead - correctedPlayerHeadPosition));

            if (canvasPlane.Raycast(ray, out float distance))
            {
                Gizmos.DrawSphere(ray.GetPoint(distance), 0.125f);
            }
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(new Vector3(0, 0, 0), _radius);
            Gizmos.DrawWireSphere(new Vector3(0, -4f, 0), _radius);
        }
    }
}