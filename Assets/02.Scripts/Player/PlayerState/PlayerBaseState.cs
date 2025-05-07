using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState : IState
{
    protected PlayerStateMachine _stateMachine;

    protected Vector2 _currentVelocity;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {

    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = _stateMachine.Player.Input;
        input.PlayerInputActions.Player.Move.performed += OnMovementPerformed;
        input.OnFireInput.AddListener(OnAttackPerformed);
        input.OnJumpInput.AddListener(OnJumpButtonPressed);
        input.OnDashInputStart.AddListener(OnDashButtonPressed);
        input.OnDashInputEnd.AddListener(OnDashButtonReleased);
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = _stateMachine.Player.Input;
        input.PlayerInputActions.Player.Move.canceled -= OnMovementCanceled;
        input.OnFireInput.RemoveListener(OnAttackPerformed);
        input.OnJumpInput.RemoveListener(OnJumpButtonPressed);
        input.OnDashInputStart.RemoveListener(OnDashButtonPressed);
        input.OnDashInputEnd.RemoveListener(OnDashButtonReleased);
    }

    protected void OnDashButtonPressed()
    {
        _stateMachine.ChangeState(_stateMachine.DashState);
    }

    protected virtual void OnDashButtonReleased()
    {
        if (_stateMachine.CurrentState is PlayerDashState dashState)
        {
            dashState.OnDashRelease();
        }
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        _stateMachine.MovementInput = context.ReadValue<Vector2>();
        if (_stateMachine.MovementInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) 
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }


    protected void OnJumpButtonPressed()
    {
        _stateMachine.ChangeState(_stateMachine.JumpState);
    }

    protected virtual void OnAttackPerformed()
    {
        _stateMachine.ChangeState(_stateMachine.AttackState);
    }

    protected void StartAnimation(int animatorHash)
    {
        _stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
         _stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    protected void ReadMovementInput()
    {
        _stateMachine.MovementInput = _stateMachine.Player.Input.PlayerInputActions.Player.Move.ReadValue<Vector2>();
    }
}
