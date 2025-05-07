using UnityEngine;
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.MovementInput.sqrMagnitude <= 0.01f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        Vector3 moveDir = new Vector3(_stateMachine.MovementInput.x, 0f, _stateMachine.MovementInput.y).normalized;
        float speed = _stateMachine.MovementSpeed * _stateMachine.MovementSpeedModifier;

        _stateMachine.Rigidbody.velocity = moveDir * speed + Vector3.up * _stateMachine.Rigidbody.velocity.y;
    }
}
