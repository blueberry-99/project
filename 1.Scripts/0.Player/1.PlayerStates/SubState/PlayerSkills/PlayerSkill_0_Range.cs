using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_0_Range : PlayerState
{
    private int xInput;

    private bool flipInput;

    private bool castDelayDone;
    private float castDelayCounter;

    private bool fireDone;
    private float fireDurationCounter;

    //About Movement(Decel Velocity)
    private Vector2 currentVelocity;
    Vector2 targetVelocity;
    private Vector2 velocity = Vector2.zero;

    GameObject obj; //for Saving Effects

    public PlayerSkill_0_Range(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        
        obj = PlayerAttachedEffectPool.instance.GetFromPool("SkillEffect_Range_0", Quaternion.identity);
        Player.SetGravityScale(PlayerData.gravityScale);
        castDelayDone = false;
        fireDone = false;
        flipInput = false;

        currentVelocity = Player.RB.velocity;

        castDelayCounter = PlayerData.skill_0_CastDelay;
        fireDurationCounter = PlayerData.skill_0_FireDuration;
        PlayerSounds.instance.PlaySound("Skill_Range_0_Pre");
    
        Player.PlayAnimation("Skill_0_Pre");

        //Start Flip
        xInput = Player.InputHandler.NormInputX;
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();
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
            if (fireDone == false)
            {
                fireDone = true;
                // PlayerParticleManager.ParticleManager.PlayParticle("8.Skill_Range_Splash");
                PlayerAttachedEffectPool.instance.GetFromPool("SkillEffect_Range_Slash", Quaternion.Euler(0, 0, 0 ));
                GameManager.gameManager.CameraShake();

                Player.RB.velocity = new Vector2((Player.isFacingRight) ? -7 : 7, 7);
                //Player.RB.AddForce(new Vector2 (7, 7), ForceMode2D.Impulse);
                Player.PlayAnimation("Skill_0_Fire");
                CreateProjectile();
                PlayerSounds.instance.PlaySound("Skill_Range_0_Fire");
                
            }

            if (fireDurationCounter >= 0)
            {
                fireDurationCounter -= Time.deltaTime;
            }
            else
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }

        //Get Flip Inputs
        if (Player.CheckIfShouldFlip(xInput)) flipInput = true;
    }

    private void CreateProjectile()
    {
        float rotation = (Player.isFacingRight) ? 0 : 180;
        
        GameObjectPool.gameObjectPool.GetFromPool("SkillProjectile", Quaternion.Euler(0, 0, rotation));
    }

    private void DecelCurrentVelocity()
    {
        currentVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref velocity, 0.1f);
        Player.RB.velocity = currentVelocity;
    }

    public override void Exit()
    {
        base.Exit();
        //PlayerParticleManager.ParticleManager.StopParticle("8.Skill_Range_Splash");
        //Exit Flip
        if (flipInput) Player.Flip();
        Player.DecreaseAmoutOfJumpsLeft();

        //End Effect
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            PlayerAttachedEffectPool.instance.ReturnToPool(obj);
        }
    }
}
