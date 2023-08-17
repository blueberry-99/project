using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipState : PlayerGroundState
{
    public PlayerFlipState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(player, playerStateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.Flip();
        Player.PlayAnimation("Flip");
        PlayerParticleManager.ParticleManager.PlayParticle("0.Move_Dust");
    }

    public override void Exit()
    {
        base.Exit();
        PlayerParticleManager.ParticleManager.StopParticle("0.Move_Dust");
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
            if (xInput == 0)
            {
                //대시 중엔 stopstate로 전환되지 않게 해야 함...
                //flip중에 대시...
                StateMachine.ChangeState(Player.StopState);
            }
            else if (isAnimationFinished)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
        }
    }
}
