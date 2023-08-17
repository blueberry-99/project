using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(player, playerStateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.PlayAnimation("Idle");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if 
        (
            !(JumpInput && Player.CanJump()) 
            && isGrounded 
            && !(DashInputHold) 
            && !(AttackInput && Player.CanAttack())
            && !Player.DashState.isDashing
            && !(SkillInput && PlayerManager.instance.IsManaFull())
            && !(Player.CanSave() && yInput == 1)
        )
        {
            if (xInput != 0)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
        }

    }
}
