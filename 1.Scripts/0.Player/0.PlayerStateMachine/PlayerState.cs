using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player Player;
    protected PlayerStateMachine StateMachine;
    protected PlayerData PlayerData;

    protected bool isAnimationFinished;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData)
    {
        this.Player = player;
        this.StateMachine = stateMachine;
        this.PlayerData = playerData;
    }

    public virtual void Enter()
    {
        DoCheck();
        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }

    public virtual void DoCheck()
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationTrigger2() { }

    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }
}
