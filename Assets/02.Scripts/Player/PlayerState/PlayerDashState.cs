using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _baseSpeed;

    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _baseSpeed = _stateMachine.MovementSpeed;
        _stateMachine.MovementSpeed *= _stateMachine.DashSpeedMultiplier;

        StartAnimation(_stateMachine.Player.AnimationData.DashParameterHash);
    }

    public override void PhysicsUpdate()
    {
        Move(allowRotation: false); // ���� �Է� �������� �ӵ��� ����
    }

    public override void Update()
    {
        // Dash ���� �ð� ����, ��� ������ �־�� ������
    }

    public override void LateUpdate()
    {
        base.LateUpdate(); // ī�޶� ����� ����ȭ
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.MovementSpeed = _baseSpeed;
        StopAnimation(_stateMachine.Player.AnimationData.DashParameterHash);
    }

    //  �� �޼���� ��ư�� ���� ���� ȣ���
    public void OnDashButton()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }


}
