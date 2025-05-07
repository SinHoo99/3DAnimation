using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    public int IdleParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    public int DashParameterHash { get; private set; }

    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(ParameterHash.Idle);
        WalkParameterHash = Animator.StringToHash(ParameterHash.Walk);
        JumpParameterHash = Animator.StringToHash(ParameterHash.Jump);
        AttackParameterHash = Animator.StringToHash(ParameterHash.Attack);
        DashParameterHash = Animator.StringToHash(ParameterHash.Dash);
    }
}
