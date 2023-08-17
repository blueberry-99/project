using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirAttackState : PlayerAbilityState
{
    private bool upInput;
    private bool downInput;

    private bool jumpInputStop;

    private bool isGrounded;

    private float accelerationInAir;
    private float decelerationInAir;

    private GameObject obj;

    private float attackDurationTimeCounter;
    private bool isAnimationLeastLengthPlayed;

    //for Recoil and defines the current attack
    private bool isDownAttacking;
    private bool isUpAttacking;
    private bool isSideAttacking;

    public PlayerInAirAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
        attackDurationTimeCounter = PlayerData.baseAttackDurationTime;

        xInput = Player.InputHandler.NormInputX;
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();

        //Set Type of attacks & create attack
        if (upInput)
        {
            isUpAttacking = true;
            Player.PlayAnimation("UpAttack_Air");
            CreateEffectByPlayerDirection("InAirAttackEffect_Up");
        }
        else if (downInput)
        {
            isDownAttacking = true;
            Player.PlayAnimation("DownAttack_Air"); 
            CreateEffectByPlayerDirection("InAirAttackEffect_Down");
        }
        else
        {
            Player.PlayAnimation("SideAttack_Air");
            isSideAttacking = true;
            CreateEffectByPlayerDirection("InAirAttackEffect_Side");
        }
    }

    public override void Exit()
    {
        base.Exit();
        isAnimationLeastLengthPlayed = false;
        Player.SetAttackCoolDown(PlayerData.inAirAttackCoolDown);

        isUpAttacking = false;
        isDownAttacking = false;
        isSideAttacking = false;

        if (obj.activeSelf)
            {
                obj.SetActive(false);
                PlayerAttachedEffectPool.instance.ReturnToPool(obj);
            }

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        GetInputs();
        CheckJumpInputStop();
        attackDurationTimeCounter -= Time.deltaTime;

        //플레이어가 땅에 닿았을 때, 애니메이션 최소 길이만큼 재생되고 LandState로
        //최소 공격 프레임이 재생된 이후 전환을 위한 것.
        if (isGrounded && Player.CurrentVelocity.y < 0.01f && isAnimationLeastLengthPlayed)
        {
            StateMachine.ChangeState(Player.LandState);
            //End Effect
            /* if (obj.activeSelf)
            {
                obj.SetActive(false);
                PlayerAttachedEffectPool.AttachedEffectPool.ReturnToPool(obj);
            } */
        }
        else if (attackDurationTimeCounter < 0)
        {
            Player.SetAttackCoolDown(PlayerData.inAirAttackCoolDown);
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Player.MoveX(accelerationInAir, decelerationInAir);
    }

    public void SetCustomInAirAccelWhileAttack(float accel, float decel)
    {
        accelerationInAir = accel;
        decelerationInAir = decel;
    }


    private void GetInputs()
    {
        jumpInputStop = Player.InputHandler.JumpInputStop;
    }


    private void CheckJumpInputStop()
    {
        if (jumpInputStop)
        {
            Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
        }
    }

    public void SetYInput(int yInput)
    {
        if (yInput == 1)
        {
            upInput = true;
            downInput = false;
        }
        else if (yInput == -1)
        {
            downInput = true;
            upInput = false;
        }
        else
        {
            upInput = false;
            downInput = false;
        }
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (isUpAttacking)
        {

        }
        else if (isDownAttacking)
        {
            Collider2D[] HittedEnemies;
            HittedEnemies = Player.CheckAttackHitted("InAir_Down");
            if (HittedEnemies.Length > 0)
            {
                StartRecoil("InAirDownAttack");
                foreach (Collider2D HittedEnemy in HittedEnemies)
                {
                    if (HittedEnemy.TryGetComponent(out Enemy Enemy))
                    {
                        /* Enemy.SetHitSlashEffect(Player.ID_Slash.position, Quaternion.Euler(0, (Player.isFacingRight ? 0 : 180), PlayerData.Down_Slash_R));
                        Enemy.SetHitBackEffect(Player.ID_HitBack.position, Quaternion.Euler(0, (Player.isFacingRight ? 0 : 180), PlayerData.Down_HitBack_R));
                        Enemy.SetHitBackParticle(Player.ID_HitParticle.position, Quaternion.Euler(0, (Player.isFacingRight ? 0 : 180), PlayerData.Down_HitParticle_R));
                        Enemy.AttackHitted(0, -1); */
                    }
                }
                /* Player.InAirState.SetOtherAnimation("RecoilY", 1f);
                isAbilityDone = true; */
            }
        }
        else if (isSideAttacking)
        {
            Collider2D[] HittedEnemies;
            HittedEnemies = Player.CheckAttackHitted("InAir_Side");
            if (HittedEnemies.Length > 0)
            {
                int attackdir = (Player.isFacingRight) ? 1 : -1;
                StartRecoil("InAirSideAttack");

                foreach (Collider2D HittedEnemy in HittedEnemies)
                {
                    /* if (HittedEnemy.TryGetComponent(out Enemy Enemy))
                    {
                        Enemy.SetHitSlashEffect(Player.IS_Slash.position, Quaternion.Euler(0, (Player.isFacingRight ? 0 : 180), PlayerData.IS_Slash_Rotation));
                        Enemy.SetHitBackEffect(Player.IS_HitBack.position, Quaternion.Euler(0, (Player.isFacingRight ? 0 : 180), PlayerData.IS_HitBack_Rotation));
                        Enemy.SetHitBackParticle(Player.IS_HitParticle.position, Quaternion.Euler(0, (Player.isFacingRight ? 0 : 180), PlayerData.IS_HitParticle_Rotation));
                        Enemy.AttackHitted(attackdir, 0);
                    } */
                }
            }
        }
        else
        {
            Debug.Log("오류");
        }

    }

    public override void AnimationTrigger2()
    {
        base.AnimationTrigger2();
        isAnimationLeastLengthPlayed = true;
    }

    private void CreateEffectByPlayerDirection(string name)
    {
        if (!Player.isFacingRight)
        {
            obj = PlayerAttachedEffectPool.instance.GetFromPool(name, Quaternion.identity);
        }
        else
        {
            obj = PlayerAttachedEffectPool.instance.GetFromPool(name, Quaternion.identity);
        }
    }

    private void StartRecoil(string nameOfAttack)
    {
        //Reset Velocity & Gravity to always perform same recoil
        Player.RB.velocity = new Vector2(Player.RB.velocity.x * 0.5f, 0);
        Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);

        if (nameOfAttack.Equals("InAirUpAttack"))
        {

        }
        /* else if (nameOfAttack.Equals("InAirDownAttack"))
        {
            Player.ResetCanAirDash();
            Player.StartYRecoil(PlayerData.inAirDownAttackRecoilForce.y, PlayerData.inAirSideAttackRecoilTime);
            //SetCustomInAirAccelWhileAttack(PlayerData.inAirDownAttackAccelerationInAir, PlayerData.inAirDownAttackDecelerationInAir);
            //Player.InAirState.SetCustomInAirAccel
            //(PlayerData.inAirDownAttackAccelerationInAir, PlayerData.inAirDownAttackDecelerationInAir, PlayerData.inAirDownAttackCustomInAirAccelTime);
        }
        else if (nameOfAttack.Equals("InAirSideAttack"))
        {
            Player.StartXRecoil(PlayerData.inAirSideAttackRecoilForce.x, PlayerData.inAirSideAttackRecoilTime);
            //set custom InAirAcceleration while during Attack & InAir
            SetCustomInAirAccelWhileAttack(PlayerData.accelerationInAir, PlayerData.decelerationInAir);
        } */
        else
        {
            Debug.Log("오류");
        }
    }
}
