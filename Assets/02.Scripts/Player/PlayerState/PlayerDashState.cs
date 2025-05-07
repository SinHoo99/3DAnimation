using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _dashSpeed = 12f;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // Dash �ִϸ��̼� Ʈ���� �ʿ�� ���⼭ ����
        // StartAnimation(_stateMachine.Player.AnimationData.DashParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        // StopAnimation(...) �ʿ� �� ó��
    }

    public override void PhysicsUpdate()
    {
        // �Է� ���� �������� ���
        Vector3 input = _stateMachine.MovementInput.normalized;
        Vector3 dashVelocity = input * _dashSpeed;

        // Y �ӵ� ����
        dashVelocity.y = _stateMachine.Rigidbody.velocity.y;

        _stateMachine.Rigidbody.velocity = dashVelocity;
    }

    /// Dash ��ư�� ���� �� ȣ���
    public void OnDashRelease()
    {
        // �̵� ���̸� Walk, �ƴϸ� Idle ���·� ����
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
