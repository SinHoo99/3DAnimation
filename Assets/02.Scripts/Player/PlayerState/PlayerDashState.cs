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
        velocity.y = _stateMachine.Rigidbody.velocity.y; // �߷��� ����
        _stateMachine.Rigidbody.velocity = velocity;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        // ��� �߿� ���� ���� ������Ʈ ���� (FixedUpdate���� �ӵ� ������)
    }

    public void OnDashRelease()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
