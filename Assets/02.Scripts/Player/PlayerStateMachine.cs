using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Vector2 MovementInput { get; set; }
    public Vector2 AttackInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerJumpState JumpState { get; }
    public PlayerFireState AttackState { get; }

    public Rigidbody Rigidbody => Player.Rigidbody;
    public string CurrentStateName => _currentState?.GetType().Name ?? "None"; 

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        JumpState = new PlayerJumpState(this);
        AttackState = new PlayerFireState(this);
        MovementSpeed = 10f;
    }

}
