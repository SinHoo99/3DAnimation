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
        
    }

    public virtual void LateUpdate()
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
    #region 움직임
    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        _stateMachine.MovementInput = context.ReadValue<Vector2>();

        // 점프나 대시 중에는 입력으로 상태 변경하지 않음
        if (_stateMachine.CurrentState is PlayerJumpState || _stateMachine.CurrentState is PlayerDashState || _stateMachine.CurrentState is PlayerFireState)
            return;

        if (_stateMachine.MovementInput.magnitude > 0.1f)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
        }
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    #endregion

    #region 점프
    protected void OnJumpButtonPressed()
    {
        _stateMachine.PreviousState = _stateMachine.CurrentState;
        _stateMachine.ChangeState(_stateMachine.JumpState);
    }
    #endregion

    #region 대쉬
    protected void OnDashButtonPressed()
    {
        if (_stateMachine.CurrentState is PlayerJumpState) return;
        _stateMachine.ChangeState(_stateMachine.DashState);
    }

    protected virtual void OnDashButtonReleased()
    {
        if (_stateMachine.CurrentState is PlayerDashState dashState)
        {
            dashState.OnDashButton();
        }
    }
    #endregion

    #region 공격
    protected virtual void OnAttackPerformed()
    {
        Debug.Log("Attack input triggered. Changing to AttackState.");
        _stateMachine.ChangeState(_stateMachine.AttackState);
    }

    #endregion

    #region 애니메이션
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

    #endregion

    #region 유틸
    protected void ReadMovementInput()
    {
        _stateMachine.MovementInput = _stateMachine.Player.Input.PlayerInputActions.Player.Move.ReadValue<Vector2>();
    }

    protected void Move(bool allowRotation = true)
    {
        Transform transform = _stateMachine.Player.transform;
        Vector2 input = _stateMachine.MovementInput;

        float baseSpeed = _stateMachine.MovementSpeed * _stateMachine.MovementSpeedModifier;

        // 방향 벡터 계산
        Vector3 moveDir = (transform.right * input.x + transform.forward * input.y).normalized;

        // 카메라 기준 전방 벡터와 moveDir의 각도 계산
        float forwardDot = Vector3.Dot(transform.forward, moveDir); // -1 ~ 1

        // 전방이 아니면 속도 줄이기
        float speedModifier = forwardDot < 0.7f ? 0.5f : 1f; // 45도 이상 벗어나면 속도 절반

        float moveSpeed = baseSpeed * speedModifier;

        Vector3 horizontalVelocity = moveDir * moveSpeed;
        float yVelocity = _stateMachine.Rigidbody.velocity.y;
        Vector3 finalVelocity = new Vector3(horizontalVelocity.x, yVelocity, horizontalVelocity.z);

        _stateMachine.Rigidbody.velocity = finalVelocity;
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
    #endregion


}
