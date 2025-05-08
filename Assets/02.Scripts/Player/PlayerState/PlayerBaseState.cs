using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState : IState
{
    protected PlayerStateMachine _stateMachine;

    protected Vector2 _currentVelocity;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
        SyncCharacterRotationToCamera();
    }

    protected virtual void AddInputActionCallbacks()
    {
        PlayerController input = _stateMachine.Player.Input;
        input.PlayerInputActions.Player.Move.performed += OnMovementPerformed;
        input.OnFireInput.AddListener(OnAttackPerformed);
        input.OnJumpInput.AddListener(OnJumpButtonPressed);
        input.OnDashInputStart.AddListener(OnDashButtonPressed);
        input.OnDashInputEnd.AddListener(OnDashButtonReleased);
    }

    protected virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = _stateMachine.Player.Input;
        input.PlayerInputActions.Player.Move.canceled -= OnMovementCanceled;
        input.OnFireInput.RemoveListener(OnAttackPerformed);
        input.OnJumpInput.RemoveListener(OnJumpButtonPressed);
        input.OnDashInputStart.RemoveListener(OnDashButtonPressed);
        input.OnDashInputEnd.RemoveListener(OnDashButtonReleased);
    }
    protected void OnDashButtonPressed()
    {
        _stateMachine.ChangeState(_stateMachine.DashState);
    }

    protected virtual void OnDashButtonReleased()
    {
        if (_stateMachine.CurrentState is PlayerDashState dashState)
        {
            dashState.OnDashRelease();
        }
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        _stateMachine.MovementInput = context.ReadValue<Vector2>();

        // 점프 중엔 WalkState로 넘어가지 않도록 차단
        if (_stateMachine.CurrentState is PlayerJumpState) return;

        if (_stateMachine.MovementInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) 
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }


    protected void OnJumpButtonPressed()
    {
        _stateMachine.ChangeState(_stateMachine.JumpState);
    }

    protected virtual void OnAttackPerformed()
    {
        _stateMachine.ChangeState(_stateMachine.AttackState);
    }

    protected void StartAnimation(int animatorHash)
    {
        _stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        _stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    protected void SetTriggerAnimation(int animatorHash)
    {
        _stateMachine.Player.Animator.SetTrigger(animatorHash);
    }


    protected void ReadMovementInput()
    {
        _stateMachine.MovementInput = _stateMachine.Player.Input.PlayerInputActions.Player.Move.ReadValue<Vector2>();
    }

    protected bool IsGrounded()
    {
        CapsuleCollider capsule = _stateMachine.Player.GetComponent<CapsuleCollider>();
        if (capsule == null) return false;

        Vector3 origin = capsule.bounds.center;
        float distanceToBottom = capsule.bounds.extents.y;
        Vector3 groundCheckPos = origin + Vector3.down * (distanceToBottom + 0.05f);
        float radius = capsule.radius * 0.9f;

        return Physics.CheckSphere(groundCheckPos, radius, LayerMask.GetMask("Ground"));
    }

    protected void Move(bool allowRotation = true)
    {
        Transform transform = _stateMachine.Player.transform;
        Vector2 input = _stateMachine.MovementInput;
        float moveSpeed = _stateMachine.MovementSpeed * _stateMachine.MovementSpeedModifier;

        //  캐릭터 자신의 기준으로 방향 계산
        Vector3 moveDir = (transform.right * input.x + transform.forward * input.y).normalized;

        Vector3 horizontalVelocity = moveDir * moveSpeed;
        float yVelocity = _stateMachine.Rigidbody.velocity.y;
        Vector3 finalVelocity = new Vector3(horizontalVelocity.x, yVelocity, horizontalVelocity.z);
        _stateMachine.Rigidbody.velocity = finalVelocity;

        //  이동 중일 때만 캐릭터 회전
        if (allowRotation && moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    protected void SyncCharacterRotationToCamera()
    {
        Transform playerTransform = _stateMachine.Player.transform;
        Transform camTransform = Camera.main.transform;

        Vector3 camForward = camTransform.forward;
        camForward.y = 0f;
        if (camForward.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(camForward);
        playerTransform.rotation = targetRotation;
    }

}
