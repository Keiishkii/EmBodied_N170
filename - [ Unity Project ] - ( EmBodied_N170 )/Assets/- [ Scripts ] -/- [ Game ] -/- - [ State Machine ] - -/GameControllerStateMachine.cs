using UnityEngine;
using DataCollection;
using Data;

namespace StateMachine
{
    public class GameControllerStateMachine : MonoBehaviour
    {
        private State _currentState;
        
        #region States
            private readonly State_SessionStart SessionStart = new State_SessionStart();
            public  readonly State_SessionComplete SessionComplete = new State_SessionComplete();
            public  readonly State_BlockStart BlockStart = new State_BlockStart();
            public  readonly State_BlockComplete BlockComplete = new State_BlockComplete();
            public  readonly State_TrialStart TrialStart = new State_TrialStart();
            public  readonly State_TrialComplete TrialComplete = new State_TrialComplete();
            public  readonly State_LightsOn LightsOn = new State_LightsOn();
            public  readonly State_AwaitingForRoomEnter AwaitingForRoomEnter = new State_AwaitingForRoomEnter();
            public  readonly State_AwaitingPlayerChoice AwaitingPlayerChoice = new State_AwaitingPlayerChoice();
            public  readonly State_AwaitingReturnToCorridor AwaitingReturnToCorridor = new State_AwaitingReturnToCorridor();
            public  readonly State_Questionnaire Questionnaire = new State_Questionnaire();
        #endregion

        public SessionFormat SessionFormatObject;
        
        [HideInInspector] public int blockIndex, trialIndex;
        [HideInInspector] public Block currentBlock;
        [HideInInspector] public Trial currentTrial;

        public readonly DataContainer dataContainer = new DataContainer();
        public BlockData CurrentBlockData => dataContainer.blockData[blockIndex];
        public TrialData CurrentTrialData => CurrentBlockData.trialData[trialIndex];


        private void Start()
        {
            _currentState = SessionStart;
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            _currentState.Update(this);
        }

        public void SetState(State state)
        {
            _currentState.OnExitState(this);
            _currentState = state;
            _currentState.OnEnterState(this);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                _currentState.OnDrawGizmos(this);
        }
    }
}