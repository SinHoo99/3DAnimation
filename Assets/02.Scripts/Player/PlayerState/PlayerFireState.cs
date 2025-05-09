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

        // ���� Ű �Է� ���� (����)
        if (_stateMachine.Player.Input.PlayerInputActions.Player.Fire.triggered)
        {
            _comboQueued = true;
        }
    }

    // [�ִϸ��̼� �̺�Ʈ���� ȣ���]
    public void OnComboCheckEvent()
    {
        if (_comboQueued)
        {
            // �Է��� �־��ٸ� ���� Ÿ�� ��� ����
            _comboQueued = false; // �Է� ó�� �Ϸ�
            Debug.Log("Combo continued");
        }
        else
        {
            // �Է� ������ �ִϸ��̼� �߰��� ���� Ż��
            ExitToMovement();
        }
    }

    // [�ִϸ��̼� ���������� ȣ���]
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

