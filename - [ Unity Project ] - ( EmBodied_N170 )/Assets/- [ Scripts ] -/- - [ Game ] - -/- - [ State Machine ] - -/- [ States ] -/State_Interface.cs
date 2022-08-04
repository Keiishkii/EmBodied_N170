using UnityEngine;

namespace StateMachine
{
    public abstract class State_Interface
    {
        private Data.DataCollection.DataCollector _dataCollector;
        protected Data.DataCollection.DataCollector DataCollector => _dataCollector ?? (_dataCollector = GameObject.FindObjectOfType<Data.DataCollection.DataCollector>());
        
        private PlayerController _playerController;
        protected PlayerController PlayerController => _playerController ?? (_playerController = GameObject.FindObjectOfType<PlayerController>());

        protected Transform CameraOffset => PlayerController.transform;
        protected Transform CameraTransform => PlayerController.cameraTransform;
        protected Transform RightHandTransform => PlayerController.rightHandTransform;
        protected Transform LeftHandTransform => PlayerController.leftHandTransform;


        public virtual void OnEnterState(GameControllerStateMachine stateMachine) { }
        public virtual void WriteStateData(GameControllerStateMachine stateMachine) { }

        public virtual void Update(GameControllerStateMachine stateMachine) { }

        public virtual void OnExitState(GameControllerStateMachine stateMachine) { }


        public virtual void OnDrawGizmos(GameControllerStateMachine stateMachine) { }
    }
}