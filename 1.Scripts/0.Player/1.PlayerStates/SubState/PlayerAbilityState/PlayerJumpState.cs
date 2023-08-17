using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private bool attackInput;
    private int yInput;
    private bool JumpInputStop;
    private float jumpAnimationTimeCounter;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        jumpAnimationTimeCounter = PlayerData.jumpAnimationTime;
        //Logic
        Player.SetGravityScale(PlayerData.gravityScale);
        Player.AirDashCoolDown();
        Player.DecreaseAmoutOfJumpsLeft();
        Player.RB.velocity = new Vector2(Player.RB.velocity.x, PlayerData.jumpForce);

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
        attackInput = Player.InputHandler.AttackInput;
        yInput = Player.InputHandler.NormInputY;
        JumpInputStop = Player.InputHandler.JumpInputStop;
        //Attacks
        if (attackInput && Player.CanAttack())
        {
            Player.InputHandler.UseAttackInput();
            Player.BaseAttackState.SetYInput(yInput);
            StateMachine.ChangeState(Player.BaseAttackState);
        }
        else if (jumpAnimationTimeCounter < 0)
        {
            isAbilityDone = true;
        }
        else
        {

        }

        //Check fastFall
        if (JumpInputStop) Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Player.MoveX(PlayerData.accelerationInAir, PlayerData.decelerationInAir);
    }
}
