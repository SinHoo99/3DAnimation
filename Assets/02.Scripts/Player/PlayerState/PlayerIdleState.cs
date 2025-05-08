using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        Vector3 velocity = _stateMachine.Rigidbody.velocity;
        velocity.x = 0f;
        velocity.z = 0f;
        _stateMachine.Rigidbody.velocity = velocity;
        _stateMachine.JumpCount = 0;
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

        if (!IsGrounded()) return;

        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }
}
