using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _dashSpeed = 12f;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // Dash 애니메이션 트리거 필요시 여기서 실행
        // StartAnimation(_stateMachine.Player.AnimationData.DashParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        // StopAnimation(...) 필요 시 처리
    }

    public override void PhysicsUpdate()
    {
        // 입력 방향 기준으로 대시
        Vector3 input = _stateMachine.MovementInput.normalized;
        Vector3 dashVelocity = input * _dashSpeed;

        // Y 속도 유지
        dashVelocity.y = _stateMachine.Rigidbody.velocity.y;

        _stateMachine.Rigidbody.velocity = dashVelocity;
    }

    /// Dash 버튼을 뗐을 때 호출됨
    public void OnDashRelease()
    {
        // 이동 중이면 Walk, 아니면 Idle 상태로 복귀
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
