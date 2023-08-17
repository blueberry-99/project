using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerToucingWallState
{
    private bool jumpInput;
    private bool dashInput;

    private bool isToucingLeftWall;
    private bool isToucingRightWall;

    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isToucingLeftWall = Player.CheckIfToucingLeftWall();
        isToucingRightWall = Player.CheckIfToucingRightWall();
    }

    public override void Enter()
    {
        base.Enter();
        Player.SetGravityScale(0);
        Player.ResetCanAirDash();
        Player.ResetAmoutOfJumpLeft();
        //Check Back Wall Touch
        if (Player.isFacingRight && isToucingLeftWall) Player.Flip();
        else if (!Player.isFacingRight && isToucingRightWall) Player.Flip();

        //if (Player.CurrentVelocity.y < -PlayerData.wallSlideSpeed) Player.RB.velocity = new Vector2(0, 0);
        Player.PlayAnimation("WallSlide");
        if(isToucingRightWall) PlayerParticleManager.ParticleManager.PlayParticle("7.WallSlide_Dust", new Vector3(0,0,24f));
        else PlayerParticleManager.ParticleManager.PlayParticle("7.WallSlide_Dust", new Vector3(0,0,72f));
        
    }

    public override void Exit()
    {
        
        base.Exit();
        Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
        Player.DecreaseAmoutOfJumpsLeft();
        PlayerParticleManager.ParticleManager.StopParticle("7.WallSlide_Dust");
        PlayerParticleManager.ParticleManager.PlayParticleWithTime("2.Jump_Dust_Trail", 0.12f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = Player.InputHandler.JumpInput;
        dashInput = Player.InputHandler.DashInput;

        if (jumpInput && Player.CheckIfToucingWall())
        {
            if (isToucingLeftWall) Player.WallJumpState.SetWallJumpDir(1);
            else if (isToucingRightWall) Player.WallJumpState.SetWallJumpDir(-1);
            Player.InputHandler.UseJumpInput();
            StateMachine.ChangeState(Player.WallJumpState);
        } //InAirDash & Dash
        else if (dashInput && Player.CanAirDash())
        {
            Player.InAirDashState.SetCantWallSlide();
            Player.Flip();
            Player.UseAirDash();
            Player.InputHandler.UseDashInput();
            StateMachine.ChangeState(Player.InAirDashState);
        }
    }

    public override void PhysicsUpdate()
    {
        Player.AddWallSlideForce();
        base.PhysicsUpdate();
    }
}
