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

        if (_hasJumped && yVelocity <= -0.1f && grounded)
        {
            if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
                _stateMachine.ChangeState(_stateMachine.WalkState);
            else
                _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        Move();
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
}
