
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Input { get; private set; }
    private PlayerStateMachine _stateMachine;
    
    
    private Animator _animator;
    public Animator Animator => _animator;
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    private void Awake()
    {
        Input = GetComponent<PlayerController>();
        _stateMachine = new PlayerStateMachine(this);
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Update();

        Debug.Log($"현재 상태: {_stateMachine.CurrentStateName}");
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }

}
