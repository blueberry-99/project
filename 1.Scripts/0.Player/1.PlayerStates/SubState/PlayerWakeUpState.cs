using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWakeUpState : PlayerState
{
    private bool isQuickWakeUp;

    private float wakeUpTimeCounter;
    public PlayerWakeUpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        
        base.Enter();
        if (isQuickWakeUp)
        {
            Player.isInvincibe = true;
            Player.PlayAnimation("WakeUp_Quick");
        }
        else
        {
            Player.PlayAnimation("WakeUp");
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        /* wakeUpTimeCounter += Time.deltaTime;

        if (wakeUpTimeCounter >= 4.735f)
        {
            StateMachine.ChangeState(Player.IdleState);
        } */
    }

    public override void Exit()
    {
        base.Exit();
        isQuickWakeUp = false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if(isQuickWakeUp) Player.SetisInvincibe(true);
        StateMachine.ChangeState(Player.IdleState);
    }

    public void SetIsQuickWakeUp()
    {
        isQuickWakeUp = true;
    }
}
