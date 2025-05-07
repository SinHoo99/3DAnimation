using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float _jumpForce = 7f;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        if (_stateMachine.JumpCount >= _stateMachine.MaxJumpCount)
            return;

        _stateMachine.JumpCount++;

        // 조건 만족 후에만 트리거 실행
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

        if (yVelocity <= 0f && grounded)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        _stateMachine.Player.Animator.ResetTrigger(_stateMachine.Player.AnimationData.JumpParameterHash);
    }
    public bool IsGrounded()
    {
        // 발 위치에서 감지하도록 오프셋 조정
        Vector3 origin = _stateMachine.Player.transform.position + Vector3.down * 0.5f;
        float radius = 0.2f;

        return Physics.CheckSphere(origin, radius, LayerMask.GetMask("Ground"));
    }
}
