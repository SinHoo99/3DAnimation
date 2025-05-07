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
        Vector3 currentVelocity = _stateMachine.Rigidbody.velocity;

        Vector3 moveDir = new Vector3(_stateMachine.MovementInput.x, 0f, _stateMachine.MovementInput.y).normalized;
        float speed = _stateMachine.MovementSpeed * _stateMachine.MovementSpeedModifier;

        currentVelocity.x = moveDir.x * speed;
        currentVelocity.z = moveDir.z * speed;
        // y는 그대로 둠 (중력 적용)

        _stateMachine.Rigidbody.velocity = currentVelocity;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            _stateMachine.Player.transform.rotation = Quaternion.Slerp(
                _stateMachine.Player.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * 10f // 회전 속도 조절
            );
        }
    }
}
