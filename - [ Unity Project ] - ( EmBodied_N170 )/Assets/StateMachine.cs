namespace StateMachine
{
    public class FiniteStateMachine
    {
        private State _currentState;

        public readonly State_Start startState = new State_Start();
        public readonly State_LightsOn lightsOnState = new State_LightsOn();

        public void Start()
        {
            _currentState = startState;
            _currentState.OnEnterState(this);
        }

        public void Update()
        {
            _currentState.Update(this);
        }
        
        public void SetState(State state)
        {
            _currentState.OnExitState(this);
            _currentState = state;
            _currentState.OnEnterState(this);
        }
    }
}