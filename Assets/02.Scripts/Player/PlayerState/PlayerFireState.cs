using UnityEngine;

public class PlayerFireState : PlayerBaseState
{
    private bool _comboQueued = false;

    public PlayerFireState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _comboQueued = false;

        _stateMachine.Player.Animator.SetBool("Attack", true);
        Debug.Log("Entered PlayerFireState");
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Player.Animator.SetBool("Attack", false);
        Debug.Log("Exited PlayerFireState");
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // 공격 키 입력 감지 (예약)
        if (_stateMachine.Player.Input.PlayerInputActions.Player.Fire.triggered)
        {
            _comboQueued = true;
        }
    }

    // [애니메이션 이벤트에서 호출됨]
    public void OnComboCheckEvent()
    {
        if (_comboQueued)
        {
            // 입력이 있었다면 다음 타로 계속 진행
            _comboQueued = false; // 입력 처리 완료
            Debug.Log("Combo continued");
        }
        else
        {
            // 입력 없으면 애니메이션 중간에 상태 탈출
            ExitToMovement();
        }
    }

    // [애니메이션 마지막에서 호출됨]
    public void OnComboEndEvent()
    {
        ExitToMovement();
    }

    private void ExitToMovement()
    {
        if (_stateMachine.MovementInput.sqrMagnitude > 0.01f)
            _stateMachine.ChangeState(_stateMachine.WalkState);
        else
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}

