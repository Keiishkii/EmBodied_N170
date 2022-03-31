namespace StateMachine
{
    public abstract class State
    {
        public abstract void OnEnterState(FiniteStateMachine stateMachine);
        public abstract void Update(FiniteStateMachine stateMachine);
        public abstract void OnExitState(FiniteStateMachine stateMachine);
    }
}