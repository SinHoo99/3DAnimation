using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _dashSpeed = 12f;
    private Vector3 _dashDirection;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        _dashDirection = _stateMachine.Player.transform.forward;

        Vector3 velocity = _dashDirection * _dashSpeed;
        velocity.y = _stateMachine.Rigidbody.velocity.y; // 중력은 유지
        _stateMachine.Rigidbody.velocity = velocity;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        // 대시 중엔 별도 물리 업데이트 없음 (FixedUpdate에서 속도 유지됨)
    }

    public void OnDashRelease()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
