using UnityEngine;
using DataCollection;
using Data;

namespace StateMachine
{
    public class GameControllerStateMachine : MonoBehaviour
    {
        private State_Interface _currentStateInterface;
        public State_Interface CurrentStateInterface => _currentStateInterface;

        #region States
            private readonly State_SessionStart _sessionStart = new State_SessionStart();
            public  readonly State_SessionComplete SessionComplete = new State_SessionComplete();
            public  readonly State_BlockStart BlockStart = new State_BlockStart();
            public  readonly State_BlockComplete BlockComplete = new State_BlockComplete();
            public  readonly State_TrialStart TrialStart = new State_TrialStart();
            public  readonly State_TrialComplete TrialComplete = new State_TrialComplete();
            public  readonly State_LightsOn LightsOn = new State_LightsOn();
            public  readonly State_AwaitingForRoomEnter AwaitingForRoomEnter = new State_AwaitingForRoomEnter();
            public  readonly State_AwaitingObjectPlacement AwaitingObjectPlacement = new State_AwaitingObjectPlacement();
            public  readonly State_AwaitingReturnToCorridor AwaitingReturnToCorridor = new State_AwaitingReturnToCorridor();
            public  readonly State_Questionnaire Questionnaire = new State_Questionnaire();
        #endregion

        public SessionFormat SessionFormatObject;
        
        [HideInInspector] public int blockIndex, trialIndex;
        [HideInInspector] public Block currentBlock;
        [HideInInspector] public Trial currentTrial;

        

        private void Start()
        {
            _currentStateInterface = _sessionStart;
            _currentStateInterface.OnEnterState(this);
        }

        private void Update()
        {
            _currentStateInterface.Update(this);
        }

        public void SetState(State_Interface stateInterface)
        {
            _currentStateInterface.OnExitState(this);
            _currentStateInterface = stateInterface;
            _currentStateInterface.OnEnterState(this);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) _currentStateInterface.OnDrawGizmos(this);
        }
    }
}