using UnityEngine;

public class PlayerFireState : PlayerBaseState
{
    private float _fireDuration = 0.3f;
    private float _timer;

    public PlayerFireState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
     //   StartAnimation(_stateMachine.Player.AnimationData.AttackParameterHash);
        _timer = 0f;

        // Debug.Log("Fire!");
        // 공격 판정, 애니메이션 시작 등
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fireDuration)
        {
            if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
                _stateMachine.ChangeState(_stateMachine.WalkState);
            else
                _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }
}
