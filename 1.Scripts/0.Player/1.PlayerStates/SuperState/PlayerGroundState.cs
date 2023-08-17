using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    protected int xInput;
    protected int yInput;

    public bool JumpInput;
    public bool DashInputHold;
    public bool AttackInput;
    public bool SkillInput;
    public bool SkillSelectInput;
    public bool MiniMapInput;
    public bool healInput;

    protected bool isGrounded;

    public PlayerGroundState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(player, playerStateMachine, playerData)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isGrounded = Player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
        //reset Amount of Jump Left in Ground
        Player.ResetAmoutOfJumpLeft();
        Player.ResetCanAirDash();
        Player.SetGravityScale(PlayerData.gravityScale);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        GetInputs();

        //Jump
        if (JumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            StateMachine.ChangeState(Player.JumpState);
        }
        //InAir
        else if (!isGrounded)
        {
            //start coyote time when plyaer is off ground
            Player.InAirState.StartCoyoteTime();

            StateMachine.ChangeState(Player.InAirState);
        }
        //Ground Dash
        else if (DashInputHold && Player.CanDash())
        {
            Player.UseDash();
            StateMachine.ChangeState(Player.DashState);
        }
        //Ground Attack
        else if (AttackInput && Player.CanAttack())
        {
            Player.InputHandler.UseAttackInput();
            if(yInput == -1) yInput = 0;
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
        //Talk
        else if (Player.canTalk && yInput == 1)
        {
            Player.TalkState.SetTalkPosition(Player.talkPosition);
            StateMachine.ChangeState(Player.TalkState);
        }
        //Item Root
        else if (Player.canItemRoot && yInput == -1)
        {
            Player.RootState.SetItem(Player.item);
            Player.RootState.SetItemPosition(Player.itemRootPosition);
            StateMachine.ChangeState(Player.RootState);
        }
        //Heal Point
        else if (healInput && PlayerManager.instance.IsManaFull())
        {
            PlayerManager.instance.UseMana();
            Player.InputHandler.UseHealInput();
            StateMachine.ChangeState(Player.HealState);
        }
        //Skill Select
        else if (SkillSelectInput)
        {
            PlayerManager.instance.ShowSkillUI();
        }
        else if (!SkillSelectInput)
        {
            PlayerManager.instance.HideSkillUI();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //Player Can Move on Ground
        Player.MoveX(PlayerData.moveAcceleration, PlayerData.moveDeceleration);
    }

    public void GetInputs()
    {
        xInput = Player.InputHandler.NormInputX;
        yInput = Player.InputHandler.NormInputY;
        JumpInput = Player.InputHandler.JumpInput;
        DashInputHold = Player.InputHandler.DashInputHold;
        AttackInput = Player.InputHandler.AttackInput;
        SkillInput = Player.InputHandler.SkillInput;
        SkillSelectInput = Player.InputHandler.SkillSelectInput;
        healInput = Player.InputHandler.HealInput;
    }


}
