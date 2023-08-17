using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = int.MaxValue)]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    public float gravityScale; //Player RigidBody2D Gravity
    public float wallJumpGravityScale;
    public float RecoilGravityScale; //in air down attack recoil gravity
    public float fastFallGravityMultiplier; //when jump key is up

    [Header("Get Hit")]
    public float getHitTime;
    public float hitFreezeTime;
    public Vector2 knockBackVelocity;
    public float hitInvincibeTime;
    public int hitFlickerRepeatFrequency; //hit Invincible 타임 안에 몇번 깜빡일 것인가..

    [Header("For Debug")]
    public bool debugState;
    public float timeScale;

    [Header("Move")]
    public float maxMoveSpeed;
    public float moveAcceleration;
    [HideInInspector] public float accelerationAmount;
    public float moveDeceleration;
    [HideInInspector] public float decelerationAmount;
    public float accelerationInAir;
    public float decelerationInAir;

    [Header("Jump")]
    public float jumpForce;
    public int amountOfJumps = 1;
    public float jumpAnimationTime;

    [Header("DashJump")]
    public Vector2 dashJumpForce;
    public float dashJumpTime;
    public float dashJumpAccel;
    public float dashJumpDecel;
    public float dashJumpCustomAccelTime;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce;
    public float wallSlideSpeed;
    public float wallSlideAcceleration;
    [HideInInspector] public float wallSlideAccelerationAmount;
    [HideInInspector] public float wallSlideAccelerationAmount2;
    public float wallJumpCustomAccelTime;
    public float wallJumpAccelerationInAir;
    public float wallJumpDecelerationInAir;
    public float wallJumpInputStopTime;
    public float wallJumpTime;

    [Header("Dash")]
    public float dashForce;
    public float dashTime;
    public float dashDeceleration;
    public float dashCoolDown;

    [Header("Run")]
    public float runSpeed;
    public float runAcceleration;
    public float runDeceleration;
    public float runStopDownTime;
    public float runStopUpTime;

    [Header("InAirDash")]
    public Vector2 inAirDashForce;
    public float inAirDashDeceleration; //이동 속도로 부드럽게 감속
    public float inAirDashCustomAccelTime;
    public float inAirDashAccelerationInAir;
    public float inAirDashDecelerationInAir;
    public float inAirDashTime;
    public float rollSpeed;
    public float rollChangeDirAcceleration;

    [Header("Base Attack")]
    public float baseAttackDurationTime;
    public float baseAttackCoolDown;
    public float baseAttackCountResetTime;
    [HideInInspector] public float SideAttackResetTime; // baseAttackCoolDown + baseAttackCountResetTime

    public float inAirAttackCoolDown;
    public Vector2 baseSideAttackRange;
    public Vector2 baseUpAttackRange;
    public Vector2 baseDownAttackRange;

    [Header("Skill_0_Range")]
    public float skill_0_CastDelay;
    public float skill_0_FireDuration;
    public float skill_0_RecoilToEnemy;

    [Header("Skill_1_DownSmash")]
    public float skill_1_CastDelay;
    public float skill_1_SmashDuration;

    [Header("Heal")]
    public float healTime;

    [Header("Base Attack Recoil")]
    //Ground
    public float baseAttack_RecoilToEnemy;
    public float SideRecoilTime; //Can't move during side recoil
    public float SideRecoilForce;
    public float DownRecoilForce;
    public float UpRecoilForce;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.

    [Header("EffectsRotation")]
    public float Side_0_Slash_R;
    public float Side_0_HitBack_R;
    public float Side_0_HitParticle_R;
    [Space(5)]
    public float Side_1_Slash_R;
    public float Side_1_HitBack_R;
    public float Side_1_HitParticle_R;
    [Space(5)]
    public float Up_Slash_R;
    public float Up_HitBack_R;
    public float Up_HitParticle_R;
    [Space(5)]
    public float Down_Slash_R;
    public float Down_HitBack_R;
    public float Down_HitParticle_R;

    [Header("Check Variables")]
    public Vector2 groundCheckSize;
    public Vector2 wallCheckSize;
    public Vector2 dashCheckSize;
    public LayerMask whatIsGround;
    public LayerMask checkIsEnemies;

    private void OnValidate()
    {
        //Calculate the actual amount of accel/deceleration amount
        accelerationAmount = (50 * moveAcceleration) / maxMoveSpeed;
        decelerationAmount = (50 * moveDeceleration) / maxMoveSpeed;

        wallSlideAccelerationAmount = (50 * wallSlideAcceleration) / wallSlideSpeed;
        //To Calculate the reset Time properly when % calculate
        SideAttackResetTime = baseAttackCoolDown + baseAttackCountResetTime;

        #region Variable Ranges
        moveAcceleration = Mathf.Clamp(moveAcceleration, 0.01f, maxMoveSpeed);
        moveDeceleration = Mathf.Clamp(moveDeceleration, 0.01f, maxMoveSpeed);
        wallSlideAcceleration = Mathf.Clamp(wallSlideAcceleration, 0.01f, wallSlideSpeed);
        #endregion
    }
}
