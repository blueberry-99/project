using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashJumpState : PlayerAbilityState
{
    private int dir;
    //Inputs
    private bool jumpInputStop;

    //Counter variables
    private float jumpAnimationTimeCounter;
    public PlayerDashJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        jumpAnimationTimeCounter = PlayerData.jumpAnimationTime;

        //Logic
        Player.SetGravityScale(PlayerData.gravityScale);
        Player.AirDashCoolDown();
        Player.UseDash();
        Player.SetDashCoolDown();
        Player.DecreaseAmoutOfJumpsLeft();
        dir = (Player.isFacingRight) ? 1 : -1;
        Player.RB.velocity = new Vector2(PlayerData.dashJumpForce.x * dir, PlayerData.dashJumpForce.y);

        //Effect
        PlayerParticleManager.ParticleManager.PlayParticleWithTime("1.Jump_Dust", 0.1f);
        PlayerParticleManager.ParticleManager.PlayParticleWithTime("2.Jump_Dust_Trail", 0.12f);

        //Animation
        Player.PlayAnimation("Jump");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        jumpAnimationTimeCounter -= Time.deltaTime;
        jumpInputStop = Player.InputHandler.JumpInputStop;
        if (jumpInputStop) Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
        if (Player.CheckIfShouldFlip(xInput))
        {
            Player.InAirState.SetCustomInAirAccel(PlayerData.dashJumpAccel, PlayerData.decelerationInAir, PlayerData.dashJumpCustomAccelTime);
            StateMachine.ChangeState(Player.InAirState);
        }
        else if (jumpAnimationTimeCounter < 0)
        {
            Player.InAirState.SetIsDashJumping(dir);
            Player.InAirState.SetCustomInAirAccel(PlayerData.dashJumpAccel, PlayerData.decelerationInAir, PlayerData.dashJumpCustomAccelTime);
            isAbilityDone = true;
        }

    }
}
