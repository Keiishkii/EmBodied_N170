using UnityEngine;

namespace StateMachine
{
    public class State_LightsOn : State
    {
        private float _timeOfStateEntrance;
        
        public override void OnEnterState(FiniteStateMachine stateMachine)
        {
            Debug.Log("Entered State: <color=#0B0>Lights On</color>");
            _timeOfStateEntrance = Time.timeSinceLevelLoad;
        }
        
        public override void Update(FiniteStateMachine stateMachine)
        {
            if (Time.timeSinceLevelLoad - _timeOfStateEntrance > 1.0f)
            {
                stateMachine.SetState(stateMachine.startState);
            }
        }
        
        public override void OnExitState(FiniteStateMachine stateMachine)
        {
            Debug.Log("Exiting State: <color=#B00>Lights On</color>");
        }
    }
}