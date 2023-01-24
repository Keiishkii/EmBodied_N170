using System;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// The main class for controlling game logic, calls the transitions between states and the update functions for the active state.
    /// The the true logic however is encapsulated within each state, so this class mainly just manages those states.
    /// </summary>
    public class GameControllerStateMachine : MonoBehaviour
    {
        // Public and private accessors for the active experiments active state.
        private State_Interface _currentState;
        public State_Interface CurrentState
        {
            get => _currentState;
            // Used to transition between states correctly.
            set
            {
                _currentState.OnExitState(this);
                _currentState = value;
                _currentState.OnEnterState(this);
                _currentState.WriteStateData(this);
            }
        }

        #region Game States
            private readonly State_SessionStart _sessionStart = new State_SessionStart();
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

        // The input file to describe the structure of the experiment.
        // This is a ScriptableObject, designed / generated within the Session Generator editor window.
        public Data.Input.SessionFormat SessionFormatObject;
        
        // The indexes of the current block and the current trial within that block.
        [HideInInspector] public int blockIndex, trialIndex;
        // A shorthand reference to the current block from the session format object.
        [HideInInspector] public Data.Input.Block currentBlock;
        // A shorthand reference to the current trial within the current block from the session format object.
        [HideInInspector] public Data.Input.Trial currentTrial;


    

        // Sets the base state to SessionStart
        private void Start()
        {
            _currentState = _sessionStart;
            _currentState.OnEnterState(this);
        }

        // Invokes the currents states Update Function
        private void Update()
        {
            _currentState.Update(this);
        }
        
        // Invokes the currents states Gizmo Renderer function
        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            {
                if (Application.isPlaying) _currentState.OnDrawGizmos(this);
            }
            #endif
        }
    }
}