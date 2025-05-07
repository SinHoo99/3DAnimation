using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float _jumpForce = 7f;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();

        //StartAnimation(_stateMachine.Player.AnimationData.JumpParameterHash);

        Vector3 velocity = _stateMachine.Rigidbody.velocity;
        velocity.y = _jumpForce;
        _stateMachine.Rigidbody.velocity = velocity;

        // Debug.Log("Jump!");
    }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.Rigidbody.velocity.y <= 0f && IsGrounded())
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    private bool IsGrounded()
    {
        Vector3 origin = _stateMachine.Player.transform.position;
        float distance = 0.15f;
        return Physics.Raycast(origin, Vector3.down, distance, LayerMask.GetMask("Ground"));
    }
}
