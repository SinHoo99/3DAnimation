using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput inputActions;

    [Header("Movement")]
    public UnityEvent<Vector2> OnMoveInput;
    public UnityEvent OnJumpInput;
    public UnityEvent OnFireInput;


    [Header("Dash")]
    public UnityEvent OnDashInputStart;
    public UnityEvent OnDashInputEnd;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _fireAction;
    private InputAction _dashAction;
    public PlayerInput PlayerInputActions => inputActions;
    public bool IsFirePressed => inputActions.Player.Fire.WasPressedThisFrame();
    private void Awake()
    {
        inputActions = new PlayerInput();

        _moveAction = inputActions.Player.Move;
        _jumpAction = inputActions.Player.Jump;
        _fireAction = inputActions.Player.Fire;
        _dashAction = inputActions.Player.Dash;

        _moveAction.performed += context =>
        {
            Vector2 input = context.ReadValue<Vector2>();
            OnMoveInput.Invoke(input);
        };
        _moveAction.canceled += context =>
        {
            OnMoveInput.Invoke(Vector2.zero);
        };

        _jumpAction.performed += context =>
        {
            OnJumpInput.Invoke();
        };
        _fireAction.performed += context =>
        {
            OnFireInput.Invoke();
        };
        _dashAction.started += _ =>
        {
            OnDashInputStart.Invoke(); 
        };

        _dashAction.canceled += _ =>
        {
            OnDashInputEnd.Invoke(); 
        };
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

}