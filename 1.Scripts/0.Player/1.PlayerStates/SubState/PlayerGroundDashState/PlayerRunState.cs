using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundDashState
{
    private int dir;
    public PlayerRunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Set velocity to runSpeed
        dir = (Player.isFacingRight) ? 1 : -1;
        Player.RB.velocity = new Vector2(dir * PlayerData.runSpeed, Player.CurrentVelocity.y);
        Player.PlayAnimation("Run");
        PlayerParticleManager.ParticleManager.PlayParticle("5.Run_Dust");
    }

    public override void Exit()
    {
        base.Exit();
        //Player.SetDashCoolDown();
        PlayerParticleManager.ParticleManager.StopParticle("5.Run_Dust");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Player.RB.velocity = new Vector2(dir * PlayerData.runSpeed, Player.CurrentVelocity.y);
        //Jump
        if (jumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            StateMachine.ChangeState(Player.DashJumpState);
            //TODO : Change State to DashJumpState
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
        else if (!isGrounded && Player.InputHandler.canRoll)
        {
            Player.DecreaseAmoutOfJumpsLeft();
            Player.StateMachine.ChangeState(Player.RollingState);
        }
        else if (!dashInputHold)
        {
            StateMachine.ChangeState(Player.RunStopState);
        }
    }



}


