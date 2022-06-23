using System.Collections;
using System.Security.Cryptography;
using DataCollection;
using Enums;
using Questionnaire;
using UnityEngine;

namespace StateMachine
{
    public class State_TrialStart : State_Interface
    {
        private RayInteractionController _rayInteractionController;
        private RayInteractionController RayInteractionController => _rayInteractionController ?? (_rayInteractionController = GameObject.FindObjectOfType<RayInteractionController>());

        private MainCanvas _mainCanvas;
        private MainCanvas MainCanvas => _mainCanvas ?? (_mainCanvas = GameObject.FindObjectOfType<MainCanvas>());

        private HandAnimationController _handAnimationController;
        private HandAnimationController HandAnimationController => _handAnimationController ?? (_handAnimationController = GameObject.FindObjectOfType<HandAnimationController>());

        private NPCManager _npcManager;
        private NPCManager NPCManager => _npcManager ?? (_npcManager = GameObject.FindObjectOfType<NPCManager>());



        public override void OnEnterState(GameControllerStateMachine stateMachine)
        {
            int blockIndex = stateMachine.blockIndex, trialIndex = stateMachine.trialIndex;
            
            Debug.Log($"Entered State: <color=#FFF>Trial Start: {trialIndex}</color>");
            DataCollector.dataContainer.dataEvents.Add(new DataCollectionEvent_RecordMarker()
            {
                timeSinceProgramStart = Time.realtimeSinceStartup,
                currentState = $"Trial {trialIndex} Start"
            });
            
            
            MainCanvas.LookTarget.SetActive(false);
            MainCanvas.ReadyPanelVisible = true;
            
            stateMachine.currentTrial = stateMachine.currentBlock.trials[trialIndex];
            Data.Trial currentTrial = stateMachine.currentTrial;
            stateMachine.dataContainer.blockData[blockIndex].trialData.Add(new TrialData());

            RayInteractionController.RayVisibility = false;

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
            
            stateMachine.StartCoroutine(LightsOnState(stateMachine, Random.Range(2.0f, 2.5f)));
        }

        public override void Update(GameControllerStateMachine stateMachine) { }

        public override void OnExitState(GameControllerStateMachine stateMachine)
        {
            MainCanvas.ReadyPanelVisible = false;
        }
        
        private bool GetLookTargetPosition(out Vector3 position)
        {
            position = new Vector3();
            
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
            
            
            
            Plane canvasPlane = new Plane(mainCanvasTransform.forward * -1, mainCanvasPosition);
            Ray ray = new Ray(positionOfHead, Vector3.Normalize(positionOfNPCHead - correctedPlayerHeadPosition));

            if (canvasPlane.Raycast(ray, out float distance))
            {
                position = ray.GetPoint(distance);
                return true;
            }
            
            return false;
        }

        private IEnumerator LightsOnState(GameControllerStateMachine stateMachine, float waitTime)
        {
            Transform playerHeadTransform = PlayerController.cameraTransform;
            Transform lookTargetTransform = MainCanvas.LookTarget.transform;

            yield return null;
            MainCanvas.LookTarget.SetActive(true);
            
            bool isLookingAtDot = false;
            while (!isLookingAtDot)
            {
                if (GetLookTargetPosition(out Vector3 position))
                {
                    lookTargetTransform.position = position;
                }
                
                Vector3 
                    targetPosition = lookTargetTransform.position, 
                    playerHeadPosition = playerHeadTransform.position;

                float result = Vector3.Dot(
                    PlayerController.cameraTransform.forward,
                    Vector3.Normalize(targetPosition - playerHeadPosition));
                
                if (result > 0.995f) isLookingAtDot = true;
                
                yield return null;
            }
            
            Debug.Log("Looking at UI cross");
            yield return new WaitForSeconds(waitTime);
            
            stateMachine.SetState(stateMachine.LightsOn);
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
        }
    }
}