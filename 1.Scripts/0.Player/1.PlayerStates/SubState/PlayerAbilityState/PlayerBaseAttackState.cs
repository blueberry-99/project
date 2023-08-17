using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseAttackState : PlayerAbilityState
{
    private Enemy Enemy;

    private bool onHit;
    private bool SkillInput;
    private bool isSkillInputPressed;

    private bool isRecoiled;

    private int yInput;

    private int attackdirX;
    private int attackdirY;

    private bool jumpInput;
    private bool jumpInputStop;
    public bool sideFirstAttack;

    private bool isUpAttacking;
    private bool isSideAttacking;
    private bool isDownAttacking;

    private float attackDurationTimeCounter;

    private float recoilGravityScale;
    private bool isDownRecoiling;

    private Vector2 SlashEffectPos;
    private Quaternion SlashEffectRotation;

    private Vector2 HitBackEffectPos;
    private Quaternion HitBackEffectRotation;

    private Vector2 HitBackParticlePos;
    private Quaternion HitBackParticleRotation;

    //EX
    //overrided isGround > for triggering
    private bool isGrounded;

    private bool isGroundTouched;

    private bool isGroundAttack;

    private Collider2D[] HittedEnemies;
    private Collider2D[] HittedGround;
    private List<Collider2D> HittedEnemyMemoryList = new List<Collider2D>();
    private GameObject obj;

    private bool flipInput;



    public PlayerBaseAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Player.isInvincibe = true;
        onHit = false;
        Player.CreateRipple();
        isRecoiled = false;
        isSkillInputPressed = false;

        if (Player.CheckIfGrounded()) isGrounded = true;
        else isGrounded = false;

        xInput = Player.InputHandler.NormInputX;
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();

        flipInput = false;
        attackDurationTimeCounter = PlayerData.baseAttackDurationTime;
        //Reset the sideFirst Attack with Time
        if (Player.CheckIfGroundAttackResetTimeEnded()) sideFirstAttack = false;

        //Sort Attack By Setted Input
        if (yInput == 1) isUpAttacking = true;
        else if (yInput == -1) isDownAttacking = true;
        else isSideAttacking = true;

        if (isUpAttacking)
        {
            Player.PlayAnimation("BaseAttack_Up");
            obj = PlayerAttachedEffectPool.instance.GetFromPool("BaseAttackEffect_Up", Quaternion.identity);
        }
        if (isDownAttacking)
        {
            Player.PlayAnimation("BaseAttack_Down");
            obj = PlayerAttachedEffectPool.instance.GetFromPool("BaseAttackEffect_Down", Quaternion.identity);
        }
        if (isSideAttacking)
        {
            if (!sideFirstAttack)
            {
                Player.PlayAnimation("BaseAttack_Side_0");
                obj = PlayerAttachedEffectPool.instance.GetFromPool("BaseAttackEffect_Side_0", Quaternion.identity);
            }
            else
            {
                if (Player.CheckIfGrounded()) Player.PlayAnimation("BaseAttack_Side_1");
                else Player.PlayAnimation("BaseAttack_Side_1_Air");
                obj = PlayerAttachedEffectPool.instance.GetFromPool("BaseAttackEffect_Side_1", Quaternion.identity);
            }
        }
        SetEffect();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        SkillInput = Player.InputHandler.SkillInput;
        jumpInput = Player.InputHandler.JumpInput;
        jumpInputStop = Player.InputHandler.JumpInputStop;
        ChangeGravity();
        if (onHit) AttackLogic();

        attackDurationTimeCounter -= Time.deltaTime;

        if (SkillInput) isSkillInputPressed = true;

        if (Player.CheckIfShouldFlip(xInput)) flipInput = true;
        //trigger
        if (isGrounded)
        {
            if (!Player.CheckIfGrounded())
            {
                isGrounded = false;
            }
        }
        if (!isGrounded)
        {
            if (Player.CheckIfGrounded())
            {
                Player.ResetAmoutOfJumpLeft();
                Player.ResetCanAirDash();
                isGrounded = true;
            }
        }

        if (jumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            JumpWhileAttack();
        }

        if (attackDurationTimeCounter < 0)
        {
            if (isSkillInputPressed)
            {
                if (PlayerManager.instance.IsManaFull() && PlayerManager.instance.canSkill)
                {
                    PlayerManager.instance.UseMana();
                    Player.InputHandler.UseSkillInput();
                    Player.SkillState.SetInput(xInput, yInput);
                    StateMachine.ChangeState(Player.SkillState);
                }
            }
            else
            {
                isAbilityDone = true;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Player.MoveX(PlayerData.accelerationInAir, PlayerData.decelerationInAir);
    }

    public override void Exit()
    {
        base.Exit();
        Player.DecreaseAmoutOfJumpsLeft();
        Player.SetAttackCoolDown(PlayerData.baseAttackCoolDown);
        Player.SetSideAttackResetTime();
        sideFirstAttack = (sideFirstAttack) ? false : true;

        //End Effect
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            PlayerAttachedEffectPool.instance.ReturnToPool(obj);
        }

        isRecoiled = false;
        isDownRecoiling = false;

        isUpAttacking = false;
        isSideAttacking = false;
        isDownAttacking = false;

        //After Flip
        if (flipInput) Player.Flip();

        //Memory Clear
        onHit = false;
        HittedEnemyMemoryList.Clear();
    }

    public void SetYInput(int input) => yInput = input;

    #region Attack Triggered
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        onHit = true;
        //Player.isInvincibe = false;
    }
    public override void AnimationTrigger2()
    {
        base.AnimationTrigger2();
        //Player.isInvincibe = false;
        onHit = false;
    }
    #endregion

    private void AttackLogic()
    {
        if (isSideAttacking)
        {
            HittedEnemies = Player.CheckAttackHitted("Side");
            HittedGround = Player.CheckGroundHitted("Side");
        }
        else if (isUpAttacking)
        {
            HittedEnemies = Player.CheckAttackHitted("Up");
        }
        else if (isDownAttacking)
        {
            HittedEnemies = Player.CheckAttackHitted("Down");
        }

        


        if (HittedEnemies.Length > 0 && !isRecoiled)
        {
            isRecoiled = true;
            if (isSideAttacking) Player.StartXRecoil(PlayerData.SideRecoilForce, PlayerData.SideRecoilTime);
            else if (isUpAttacking)
            {
                Player.StartYRecoil(PlayerData.UpRecoilForce);
            }

            else if (isDownAttacking)
            {
                isDownRecoiling = true;
                Player.StartYRecoil(PlayerData.DownRecoilForce);
                Player.ResetCanAirDash();
            }
        }
        // 공격 판정 로직 실행
        foreach (Collider2D HittedEnemy in HittedEnemies)
        {
            // 이미 기억된 적인지 확인
            if (!HittedEnemyMemoryList.Contains(HittedEnemy))
            {
                // 기억 리스트에 추가하고 공격 적용
                HittedEnemyMemoryList.Add(HittedEnemy);
                ApplyAttack(HittedEnemy);
            }
        }
    }

    private void ApplyAttack(Collider2D HittedEnemy)
    {
        Enemy = HittedEnemy.GetComponentInParent<Enemy>();
        PlayerSounds.instance.PlaySound("BaskAttack_EnemyHit_0");

        GetMana();
        Enemy.SetHitSlashEffect(SlashEffectPos, SlashEffectRotation);
        Enemy.SetHitBackEffect(HitBackEffectPos, HitBackEffectRotation);
        Enemy.SetHitBackParticle(HitBackParticlePos, HitBackParticleRotation);

        //Enemy.AttackHitted(attackdirX, attackdirY, PlayerManager.playerManager.baseAttackDamage, PlayerData.baseAttack_RecoilToEnemy);
        //For Debug
        Enemy.AttackHitted(attackdirX, attackdirY, 1, PlayerData.baseAttack_RecoilToEnemy);

    }

    private void ChangeGravity()
    {
        if (isDownRecoiling)
        {
            Player.SetGravityScale(PlayerData.RecoilGravityScale);
        }
        else if (jumpInputStop)
        {
            Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
        }
    }

    private void JumpWhileAttack()
    {
        //isGroundTouched = false;
        Player.SetGravityScale(PlayerData.gravityScale);
        Player.AirDashCoolDown();
        Player.DecreaseAmoutOfJumpsLeft();
        Player.RB.velocity = new Vector2(Player.RB.velocity.x, PlayerData.jumpForce);

        //Effect
        PlayerParticleManager.ParticleManager.PlayParticleWithTime("1.Jump_Dust", 0.1f);
        PlayerParticleManager.ParticleManager.PlayParticleWithTime("2.Jump_Dust_Trail", 0.12f);
    }

    private void GetMana()
    {
        PlayerManager.instance.GetMana();
    }

    private void SetEffect()
    {
        float yRotation = (Player.isFacingRight ? 0 : 180);
        if (isSideAttacking)
        {
            if (!sideFirstAttack)
            {
                SlashEffectPos = Player.Base_Side_0_Slash.position;
                SlashEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Side_0_Slash_R);
                HitBackEffectPos = Player.Base_Side_0_HitBack.position;
                HitBackEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Side_0_HitBack_R);
                HitBackParticlePos = Player.Base_Side_0_HitParticle.position;
                HitBackParticleRotation = Quaternion.Euler(0, yRotation, PlayerData.Side_0_HitParticle_R);
            }
            else
            {
                SlashEffectPos = Player.Base_Side_1_Slash.position;
                SlashEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Side_1_Slash_R);
                HitBackEffectPos = Player.Base_Side_1_HitBack.position;
                HitBackEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Side_1_HitBack_R);
                HitBackParticlePos = Player.Base_Side_1_HitParticle.position;
                HitBackParticleRotation = Quaternion.Euler(0, yRotation, PlayerData.Side_1_HitParticle_R);
            }
            attackdirX = (Player.isFacingRight) ? 1 : -1;
            attackdirY = 0;
        }
        else if (isUpAttacking)
        {
            SlashEffectPos = Player.Base_Up_Slash.position;
            SlashEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Up_Slash_R);
            HitBackEffectPos = Player.Base_Up_HitBack.position;
            HitBackEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Up_HitBack_R);
            HitBackParticlePos = Player.Base_Up_HitParticle.position;
            HitBackParticleRotation = Quaternion.Euler(0, yRotation, PlayerData.Up_HitParticle_R);
            attackdirX = 0;
            attackdirY = 1;
        }
        else if (isDownAttacking)
        {
            SlashEffectPos = Player.Base_Down_Slash.position;
            SlashEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Down_Slash_R);
            HitBackEffectPos = Player.Base_Down_HitBack.position;
            HitBackEffectRotation = Quaternion.Euler(0, yRotation, PlayerData.Down_HitBack_R);
            HitBackParticlePos = Player.Base_Down_HitParticle.position;
            HitBackParticleRotation = Quaternion.Euler(0, yRotation, PlayerData.Down_HitParticle_R);
            attackdirX = 0;
            attackdirY = -1;
        }
    }
}
