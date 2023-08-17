using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSleepState : PlayerState
{
    private float wakeUpTimeCounter;
    public PlayerSleepState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.PlayAnimation("Sleep");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public void WakePlayer()
    {
        StateMachine.ChangeState(Player.WakeUpState);
    }
}
