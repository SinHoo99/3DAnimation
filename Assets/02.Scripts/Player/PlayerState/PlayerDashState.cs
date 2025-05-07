using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _dashSpeed = 12f;
    private Vector3 _dashDirection;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // 이동 입력 방향 저장 (y 제거)
        Vector3 input = new Vector3(_stateMachine.MovementInput.x, 0f, _stateMachine.MovementInput.y).normalized;

        // 대시 방향이 없으면 현재 바라보는 방향으로
        _dashDirection = input == Vector3.zero
            ? _stateMachine.Player.transform.forward
            : input;

        Vector3 velocity = _dashDirection * _dashSpeed;
        velocity.y = _stateMachine.Rigidbody.velocity.y; // 기존 y 유지 (중력)
        _stateMachine.Rigidbody.velocity = velocity;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        // 중력은 Rigidbody가 처리하므로 여기서 별도 처리 불필요
        // 필요하다면 충돌 체크나 추가 제어 가능
    }

    public void OnDashRelease()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
