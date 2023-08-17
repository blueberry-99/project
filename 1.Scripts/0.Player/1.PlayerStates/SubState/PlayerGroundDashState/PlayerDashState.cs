using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerGroundDashState
{
    private int dir;

    private float dashTimeCounter;

    public bool isDashing;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.ResetAmoutOfJumpLeft();
        Player.ResetCanAirDash();
        Player.SetGravityScale(PlayerData.gravityScale);

        isDashing = true;

        dashTimeCounter = PlayerData.dashTime;

        xInput = Player.InputHandler.NormInputX;
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();

        if (xInput == 0) dir = Player.isFacingRight ? 1 : -1;
        else dir = xInput;
        //Player.RB.velocity = new Vector2(PlayerData.dashForce * dir, Player.CurrentVelocity.y);
        Player.RB.velocity = new Vector2(PlayerData.dashForce * dir, 0);

        Player.PlayAnimation("Dash");

        //Create effects
        if (dir == 1)
        {
            PlayerDetachedEffectPool.instance.GetFromPool("DashEffect", Quaternion.Euler(0, 180, 0));
            PlayerParticleManager.ParticleManager.PlayParticle("4.GroundDash_Dust", new Vector3(0, 70f, 0));
        }
        else
        {
            PlayerDetachedEffectPool.instance.GetFromPool("DashEffect", Quaternion.identity);
            PlayerParticleManager.ParticleManager.PlayParticle("4.GroundDash_Dust", new Vector3(0, 290f, 0));
        }
        PlayerParticleManager.ParticleManager.PlayParticle("5.Run_Dust");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        dashTimeCounter -= Time.deltaTime;

        //Jump
        if (jumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            StateMachine.ChangeState(Player.DashJumpState);
        }
        //Ground Attack
        else if (attackInput && Player.CanAttack() && yInput != -1)
        {
            Player.InputHandler.UseAttackInput();
            Player.BaseAttackState.SetYInput(yInput);
            StateMachine.ChangeState(Player.BaseAttackState);
        }
        //Skill
        else if (SkillInput && PlayerManager.instance.IsManaFull() && PlayerManager.instance.canSkill)
        {
            PlayerManager.instance.UseMana();
            Player.InputHandler.UseSkillInput();
            Player.SkillState.SetInput(xInput, yInput);
            StateMachine.ChangeState(Player.SkillState);
        }
        //Save
        else if (Player.CanSave() && yInput == 1)
        {
            StateMachine.ChangeState(Player.SaveState);
        }
        else if (dashTimeCounter < 0)
        {
            if (dashInputHold)
            {
                StateMachine.ChangeState(Player.RunState);
            }
            else
            {
                StateMachine.ChangeState(Player.RunStopState);
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (!isGrounded)
        {
            Player.DecreaseAmoutOfJumpsLeft();
            Player.StateMachine.ChangeState(Player.RollingState);
        }

    }

    public override void Exit()
    {
        base.Exit();
        Player.SetDashCoolDown();
        isDashing = false;
        PlayerParticleManager.ParticleManager.StopParticle("4.GroundDash_Dust");
        PlayerParticleManager.ParticleManager.StopParticle("5.Run_Dust");
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        DashDeceleration();
    }

    private void DashDeceleration()
    {
        float targetSpeed = (dir == 1) ? PlayerData.runSpeed : -PlayerData.runSpeed;
        float speedDif = targetSpeed - Player.CurrentVelocity.x;
        float movement = speedDif * PlayerData.dashDeceleration;
        Player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
