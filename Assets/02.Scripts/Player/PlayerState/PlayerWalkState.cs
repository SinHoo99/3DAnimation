using UnityEngine;
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
        // Debug.Log("Jump!");
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(_stateMachine.Player.AnimationData.WalkParameterHash);
    }

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
        var transform = _stateMachine.Player.transform;
        Vector2 input = _stateMachine.MovementInput;
        float moveSpeed = _stateMachine.MovementSpeed * _stateMachine.MovementSpeedModifier;

        //  회전 (A/D)
        if (Mathf.Abs(input.x) > 0.01f)
        {
            float turnSpeed = 180f; // deg/sec
            float rotationAmount = input.x * turnSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, rotationAmount, 0f);
        }

        // 이동 (W/S)
        Vector3 forward = transform.forward;
        Vector3 move = forward * input.y * moveSpeed;

        Vector3 velocity = _stateMachine.Rigidbody.velocity;
        velocity.x = move.x;
        velocity.z = move.z;

        _stateMachine.Rigidbody.velocity = velocity;
    }

}
