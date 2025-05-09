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
        Move(allowRotation: false); // 현재 입력 방향으로 속도만 증가
    }

    public override void Update()
    {
        // Dash 유지 시간 없이, 계속 누르고 있어야 유지됨
    }

    public override void LateUpdate()
    {
        base.LateUpdate(); // 카메라 방향과 동기화
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.MovementSpeed = _baseSpeed;
        StopAnimation(_stateMachine.Player.AnimationData.DashParameterHash);
    }

    //  이 메서드는 버튼을 뗐을 때만 호출됨
    public void OnDashButton()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }


}
