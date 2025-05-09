public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
    public void LateUpdate();
}
public abstract class StateMachine
{
    protected IState _currentState;

    public IState PreviousState { get; set; }

    public IState CurrentState => _currentState;

    public void ChangeState(IState state)
    {
        PreviousState = _currentState;
        _currentState?.Exit();
        _currentState = state;
        _currentState?.Enter();
    }

    public void HandleInput()
    {
        _currentState?.HandleInput();
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void LateUpdate()
    {
        _currentState?.LateUpdate();
    }
    public void PhysicsUpdate()
    {
        _currentState?.PhysicsUpdate();
    }
}