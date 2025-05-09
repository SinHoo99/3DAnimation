
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    private Animator _animator;
    public Animator Animator => _animator;
    [field: Header("PlayerController")]
    public PlayerController Input { get; private set; }
  
    [field: Header("PlayerStateMachine")]
    private PlayerStateMachine _stateMachine;

    [field: Header("Rigidbody")]
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    [field: Header("Weapons")]
    [SerializeField] private Sword _rightSword;
    [SerializeField] private Sword _leftSword;

    public Sword RightSword => _rightSword;
    public Sword LeftSword => _leftSword;

    private void Awake()
    {
        Input = GetComponent<PlayerController>();
        _stateMachine = new PlayerStateMachine(this);
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        AnimationData.Initialize();
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Update();

        Debug.Log($"현재 상태: {_stateMachine.CurrentStateName}");
    }
    private void LateUpdate()
    {
        StartCoroutine(DelayedStateMachineLateUpdate());
    }
    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    private IEnumerator DelayedStateMachineLateUpdate()
    {
        yield return new WaitForEndOfFrame(); 

        _stateMachine.LateUpdate(); // 이제 카메라 방향이 최신 상태!
    }

    public void ComboCheck()
    {
        if (_stateMachine.CurrentState is PlayerFireState fireState)
        {
            fireState.OnComboCheckEvent();
        }
    }

    public void ComboEnd()
    {
        if (_stateMachine.CurrentState is PlayerFireState fireState)
        {
            fireState.OnComboEndEvent();
        }
    }

    public void EnableRightSwordCollider()
    {
        _rightSword.EnableCollider();
    }

    public void DisableRightSwordCollider()
    {
        _rightSword.DisableCollider();
    }

    // 왼손 소드 콜라이더도 필요 시
    public void EnableLeftSwordCollider()
    {
        _leftSword.EnableCollider();
    }

    public void DisableLeftSwordCollider()
    {
        _leftSword.DisableCollider();
    }
}
