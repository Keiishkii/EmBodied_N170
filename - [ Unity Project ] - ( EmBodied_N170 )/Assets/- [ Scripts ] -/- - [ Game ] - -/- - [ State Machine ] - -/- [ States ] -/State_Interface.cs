using DataCollection;
using UnityEngine;

namespace StateMachine
{
    public abstract class State_Interface
    {
        private DataCollector _dataCollector;
        protected DataCollector DataCollector => _dataCollector ?? (_dataCollector = GameObject.FindObjectOfType<DataCollector>());
        
        private PlayerController _playerController;
        protected PlayerController PlayerController => _playerController ?? (_playerController = GameObject.FindObjectOfType<PlayerController>());

        protected Transform CameraOffset => PlayerController.transform;
        protected Transform CameraTransform => PlayerController.cameraTransform;
        protected Transform RightHandTransform => PlayerController.rightHandTransform;
        protected Transform LeftHandTransform => PlayerController.leftHandTransform;
        
        
        
        public abstract void OnEnterState(GameControllerStateMachine stateMachine);
        public abstract void Update(GameControllerStateMachine stateMachine);
        public abstract void OnExitState(GameControllerStateMachine stateMachine);

        public void OnDrawGizmos(GameControllerStateMachine stateMachine) { }
    }
}