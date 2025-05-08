using UnityEngine;
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
        _stateMachine.JumpCount = 0;
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (!IsGrounded()) return;

        if (_stateMachine.MovementInput.sqrMagnitude <= 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        Move();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
}
