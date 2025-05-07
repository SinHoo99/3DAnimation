using UnityEngine;

public class PlayerFireState : PlayerBaseState
{
    private float _fireDuration = 0.3f;
    private float _timer;

    public PlayerFireState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _timer = 0f;

        // Debug.Log("Fire!");
        // ���� ����, �ִϸ��̼� ���� ��
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
