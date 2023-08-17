using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDashState : PlayerState
{
    protected bool isGrounded;

    protected int xInput;
    protected int yInput;

    protected bool jumpInput;
    protected bool dashInputHold;
    protected bool attackInput;
    protected bool SkillInput;

    public PlayerGroundDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.ResetCanAirDash();
    }
    
    public override void DoCheck()
    {
        base.DoCheck();
        isGrounded = Player.CheckIfGrounded();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        jumpInput = Player.InputHandler.JumpInput;
        dashInputHold = Player.InputHandler.DashInputHold;
        attackInput = Player.InputHandler.AttackInput;
        xInput = Player.InputHandler.NormInputX;
        yInput = Player.InputHandler.NormInputY;
        SkillInput = Player.InputHandler.SkillInput;
        /* //base change of dashstates
        //Jump
        if (jumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            //StateMachine.ChangeState(Player.JumpState);
            //TODO : Change State to DashJumpState
        }
        //InAir
        else if (!isGrounded)
        {
            //start coyote time when plyaer is off ground
            Player.InAirState.StartCoyoteTime();
            Player.InAirState.SetCustomInAirAccel
            (PlayerData.wallJumpAccelerationInAir, 0.1f, 2);
            StateMachine.ChangeState(Player.InAirState);
        }
        //Ground Attack
        else if (attackInput && Player.CanAttack())
        {
            Player.InputHandler.UseAttackInput();
            //TODO : set dash ground attack y input and change state
        } */
    }

}
