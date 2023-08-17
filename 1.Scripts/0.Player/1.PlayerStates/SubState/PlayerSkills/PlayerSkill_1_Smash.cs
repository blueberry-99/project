using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_1_Smash : PlayerState
{
    private int xInput;
    private bool flipInput;

    private bool castDelayDone;
    private float castDelayCounter;

    private bool smashDone;
    private float smashDurationCounter;

    //About Movement(Decel Velocity)
    private Vector2 currentVelocity;
    Vector2 targetVelocity;
    private Vector2 velocity = Vector2.zero;

    public PlayerSkill_1_Smash(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
        flipInput = false;
        castDelayDone = false;
        smashDone = false;

        castDelayCounter = PlayerData.skill_1_CastDelay;
        smashDurationCounter = PlayerData.skill_1_SmashDuration;

        //Start Flip
        xInput = Player.InputHandler.NormInputX;
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();

        //Animation
        Player.PlayAnimation("Skill_1_Pre");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = Player.InputHandler.NormInputX;

        if (castDelayCounter < 0)
        {
            castDelayDone = true;
        }
        else
        {
            castDelayCounter -= Time.deltaTime;
            DecelCurrentVelocity();
        }

        if (castDelayDone)
        {
            if (smashDone == false)
            {
                //PlayerAttachedEffectPool.AttachedEffectPool.GetFromPool("SkillEffect_Range_Slash", Quaternion.Euler(0, 0, 0 ));
                //GameManager.gameManager.CameraShake();

                Player.RB.AddForce(new Vector2 (0, 25), ForceMode2D.Impulse);
                Player.PlayAnimation("Skill_1_Smash");
                smashDone = true;
            }

            if (smashDurationCounter >= 0)
            {
                smashDurationCounter -= Time.deltaTime;
            }
            else
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        //Get Flip Inputs
        if (Player.CheckIfShouldFlip(xInput)) flipInput = true;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(castDelayDone) Player.MoveX(PlayerData.accelerationInAir, PlayerData.decelerationInAir);
    }

    public override void Exit()
    {
        base.Exit();
        //Exit Flip
        if (flipInput) Player.Flip();

        Player.DecreaseAmoutOfJumpsLeft();
    }

    private void DecelCurrentVelocity()
    {
        currentVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref velocity, 0.1f);
        Player.RB.velocity = currentVelocity;
    }
}
