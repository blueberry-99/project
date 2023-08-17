using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundState
{
    public PlayerLandState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(player, playerStateMachine, playerData)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Player.PlayAnimation("Land");
        PlayerParticleManager.ParticleManager.PlayParticleWithTime("3.Land_Dust", 0.1f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (
            !JumpInput
            && !(DashInputHold)
            && !(AttackInput && Player.CanAttack())
            && !(SkillInput && PlayerManager.instance.IsManaFull())
            )
        {
            if (xInput != 0)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (isAnimationFinished)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }

}
