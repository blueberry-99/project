using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private bool playOtherAnimation;
    private string otherAnimation;

    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(player, playerStateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (playOtherAnimation) Player.PlayAnimation(otherAnimation);
        else Player.PlayAnimation("Bend");

        PlayerParticleManager.ParticleManager.PlayParticle("0.Move_Dust");
    }

    public override void Exit()
    {
        base.Exit();
        PlayerParticleManager.ParticleManager.StopParticle("0.Move_Dust");
        playOtherAnimation = false;
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
                StateMachine.ChangeState(Player.StopState);
            }
            else if (playOtherAnimation)
            {
                if (Player.CheckIfShouldFlip(xInput)) Player.Flip();
            }
            else if (Player.CheckIfShouldFlip(xInput))
            {
                StateMachine.ChangeState(Player.FlipState);
            }
        }
    }

    public void SetAnimation(string str)
    {
        playOtherAnimation = true;
        otherAnimation = str;
    }
}
