namespace StateMachine
{
    public abstract class State
    {
        public abstract void OnEnterState(GameControllerStateMachine stateMachine);
        public abstract void Update(GameControllerStateMachine stateMachine);
        public abstract void OnExitState(GameControllerStateMachine stateMachine);

        public void OnDrawGizmos(GameControllerStateMachine stateMachine) { }
    }
}