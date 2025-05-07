using UnityEngine;
public class PlayerJumpState : PlayerBaseState
{
    private float _jumpForce = 7f;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Vector3 velocity = _stateMachine.Rigidbody.velocity;
        velocity.y = _jumpForce;
        _stateMachine.Rigidbody.velocity = velocity;

        // Debug.Log("Jump!");
    }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.Rigidbody.velocity.y <= 0f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
