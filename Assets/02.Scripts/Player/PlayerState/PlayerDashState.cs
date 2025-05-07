using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _dashSpeed = 12f;
    private Vector3 _dashDirection;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        // �̵� �Է� ���� ���� (y ����)
        Vector3 input = new Vector3(_stateMachine.MovementInput.x, 0f, _stateMachine.MovementInput.y).normalized;

        // ��� ������ ������ ���� �ٶ󺸴� ��������
        _dashDirection = input == Vector3.zero
            ? _stateMachine.Player.transform.forward
            : input;

        Vector3 velocity = _dashDirection * _dashSpeed;
        velocity.y = _stateMachine.Rigidbody.velocity.y; // ���� y ���� (�߷�)
        _stateMachine.Rigidbody.velocity = velocity;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        // �߷��� Rigidbody�� ó���ϹǷ� ���⼭ ���� ó�� ���ʿ�
        // �ʿ��ϴٸ� �浹 üũ�� �߰� ���� ����
    }

    public void OnDashRelease()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
