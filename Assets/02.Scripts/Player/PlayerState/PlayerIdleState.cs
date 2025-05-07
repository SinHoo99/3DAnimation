using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
        // Debug.Log("Entering Idle");
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void Update()
    {
        base.Update();

        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }
}
