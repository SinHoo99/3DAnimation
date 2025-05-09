using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float _jumpForce = 7f;
    private bool _hasJumped = false;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        if (_stateMachine.JumpCount >= _stateMachine.MaxJumpCount)
            return;

        _stateMachine.JumpCount++;
        _hasJumped = true;

        SetTriggerAnimation(_stateMachine.Player.AnimationData.JumpParameterHash);

        Vector3 velocity = _stateMachine.Rigidbody.velocity;
        velocity.y = _jumpForce;
        _stateMachine.Rigidbody.velocity = velocity;
    }

    public override void Update()
    {
        base.Update();

        float yVelocity = _stateMachine.Rigidbody.velocity.y;
        _stateMachine.Player.Animator.SetFloat(
            _stateMachine.Player.AnimationData.YVelocityParameterHash, yVelocity);

        bool grounded = IsGrounded();
        _stateMachine.Player.Animator.SetBool(
            _stateMachine.Player.AnimationData.IsGroundedParameterHash, grounded);

        if (_hasJumped && grounded && yVelocity <= 0.1f)
        {
            _stateMachine.JumpCount = 0;
            if (_stateMachine.PreviousState is PlayerDashState)
            {
                _stateMachine.ChangeState(_stateMachine.DashState);
            }
            else if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            {
                _stateMachine.ChangeState(_stateMachine.WalkState);
            }
            else
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        MoveBasedOnPreviousState();
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Player.Animator.ResetTrigger(_stateMachine.Player.AnimationData.JumpParameterHash);
        _stateMachine.Player.Animator.SetBool(_stateMachine.Player.AnimationData.WalkParameterHash, false);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    private void MoveBasedOnPreviousState()
    {
        var transform = _stateMachine.Player.transform;
        var input = _stateMachine.MovementInput;
        float baseSpeed = _stateMachine.MovementSpeed;

        // 이전 상태에 따라 multiplier 설정
        float multiplier = 1f;

        if (_stateMachine.PreviousState is PlayerDashState)
            multiplier = _stateMachine.DashSpeedMultiplier;
        else if (_stateMachine.PreviousState is PlayerWalkState)
            multiplier = _stateMachine.MovementSpeedModifier;
        else if (_stateMachine.PreviousState is PlayerIdleState)
            multiplier = 0f;

        float finalSpeed = baseSpeed * _stateMachine.MovementSpeedModifier * multiplier;

        Vector3 dir = (transform.right * input.x + transform.forward * input.y).normalized;
        Vector3 velocity = dir * finalSpeed;
        velocity.y = _stateMachine.Rigidbody.velocity.y;

        _stateMachine.Rigidbody.velocity = velocity;
    }


}
