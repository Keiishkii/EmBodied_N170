using UnityEngine;

namespace StateMachine
{
    public class GameControllerStateMachine : MonoBehaviour
    {
        private State_Interface _currentState;
        public State_Interface CurrentState
        {
            get => _currentState;
            set
            {
                _currentState.OnExitState(this);
                _currentState = value;
                _currentState.OnEnterState(this);
                _currentState.WriteStateData(this);
            }
        }

        #region Game States
            public readonly State_SessionStart _sessionStart = new State_SessionStart();
            public readonly State_SessionComplete SessionComplete = new State_SessionComplete();
            public readonly State_BlockStart BlockStart = new State_BlockStart();
            public readonly State_BlockComplete BlockComplete = new State_BlockComplete();
            public readonly State_TrialStart TrialStart = new State_TrialStart();
            public readonly State_TrialComplete TrialComplete = new State_TrialComplete();
            public readonly State_LightsOn LightsOn = new State_LightsOn();
            public readonly State_AwaitingForRoomEnter AwaitingForRoomEnter = new State_AwaitingForRoomEnter();
            public readonly State_AwaitingObjectPlacement AwaitingObjectPlacement = new State_AwaitingObjectPlacement();
            public readonly State_AwaitingReturnToCorridor AwaitingReturnToCorridor = new State_AwaitingReturnToCorridor();
            public readonly State_Questionnaire Questionnaire = new State_Questionnaire();
        #endregion

        public Data.Input.SessionFormat SessionFormatObject;
        
        [HideInInspector] public int blockIndex, trialIndex;
        [HideInInspector] public Data.Input.Block currentBlock;
        [HideInInspector] public Data.Input.Trial currentTrial;


    


        private void Start()
        {
            _currentState = _sessionStart;
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            _currentState.Update(this);
            
            
            
            
        }
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) _currentState.OnDrawGizmos(this);
        }
    }
}