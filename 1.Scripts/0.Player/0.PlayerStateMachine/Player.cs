using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplatterSystem;

public class Player : MonoBehaviour
{
    #region State Variables

    //To generate PlayerStateMachine
    public PlayerStateMachine StateMachine { get; private set; }

    //SubState for PlayerGroundState
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerStopState StopState { get; private set; }
    public PlayerFlipState FlipState { get; private set; }
    public PlayerRunStopState RunStopState { get; private set; }

    //SubState for PlayerAbilityState
    public PlayerJumpState JumpState { get; private set; }
    public PlayerDashJumpState DashJumpState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerBaseAttackState BaseAttackState { get; private set; }
    public PlayerInAirDashState InAirDashState { get; private set; }
    public PlayerRollingState RollingState { get; private set; }
    public PlayerGetHitState GetHitState { get; private set; }
    public PlayerGetHitByThornState GetHitByThornState { get; private set; }

    //PlayerSkillState
    public PlayerSkillState SkillState { get; private set; }
    public PlayerSkill_0_Range SKillState_0_Range { get; private set; }
    public PlayerSkill_1_Smash SkillState_1_Smash { get; private set; }

    //PlayerSaveState
    public PlayerSaveState SaveState { get; private set; }
    public PlayerLayDownState LayDownState { get; private set; }

    //PlayerTalkState
    public PlayerTalkState TalkState { get; private set; }

    //PlayerItemRootState
    public PlayerItemRootState RootState { get; private set; }

    //PlayerHealState
    public PlayerHealState HealState { get; private set; }

    //PlayerDieState
    public PlayerDieState DieState { get; private set; }

    //SubState for PlayerGroundDashState
    public PlayerDashState DashState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    //StandAlone State
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }

    //Substate for PlayerToucingWallState
    public PlayerWallSlideState WallSlideState { get; private set; }

    //Player BehaviorState
    public PlayerWakeUpState WakeUpState { get; private set; }

    #endregion

    #region Components

    //Get Item
    public Item item;

    public NPC npc;

    public SavePointTrigger savePointTrigger;

    public PlayerData PlayerData;
    //public PlayerStats PlayerStats;
    public PlayerInputHandler InputHandler { get; private set; }

    private Animator Animator;
    //public MeshSplatterManager meshSplatterManager;
    public Transform SplatterSpawnTransform;
    public Rigidbody2D RB { get; private set; }

    //About Flickering Effects
    private SpriteRenderer SpriteRenderer;
    private Material PlayerMaterial;
    protected int strongTintFadeID;

    //EX
    //public RippleEffect RippleEffect;

    #endregion

    #region Check Transform
    [Header("CheckPos")]
    [SerializeField] private Transform frontWallCheck;
    [SerializeField] private Transform backWallCheck;
    [SerializeField] private Transform dashCheck;
    [SerializeField] private Transform BaseSideAttackPos;
    [SerializeField] private Transform BaseUpAttackPos;
    [SerializeField] private Transform BaseDownAttackPos;

    #endregion

    [Header("HitEffectPos")]
    [SerializeField] public Transform Base_Side_0_Slash;
    [SerializeField] public Transform Base_Side_0_HitBack;
    [SerializeField] public Transform Base_Side_0_HitParticle;
    [SerializeField] public Transform Base_Side_1_Slash;
    [SerializeField] public Transform Base_Side_1_HitBack;
    [SerializeField] public Transform Base_Side_1_HitParticle;
    [SerializeField] public Transform Base_Up_Slash;
    [SerializeField] public Transform Base_Up_HitBack;
    [SerializeField] public Transform Base_Up_HitParticle;
    [SerializeField] public Transform Base_Down_Slash;
    [SerializeField] public Transform Base_Down_HitBack;
    [SerializeField] public Transform Base_Down_HitParticle;

    #region Other Variables

    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workSpace;


    //About Attack
    private float attackCoolDownCounter;
    private float groundSideAttackResetTimeCounter;

    private bool isGrounded;
    public bool isFacingRight { get; private set; }
    private bool isToucingRightWall;
    private bool isToucingLeftWall;

    //About Jump
    private int amountOfJumpsLeft;

    //About Dash
    private bool canAirDash;
    private bool canDash;

    //About Recoil & Move
    private bool cantMoveRight;
    private bool cantMoveLeft;

    private bool isRecoiling;
    private Vector2 recoilForce;
    private float recoilTimeCounter;
    private int recoilDirection;

    //about Getting hitted
    private bool getHit;
    private bool getHitByThorn;

    public bool isInvincibe;

    //About Save
    private bool canSave;
    private Vector2 savePointPosition;

    //About Interaction
    public bool canTalk;
    public Vector2 talkPosition;

    //About Item Root
    public bool canItemRoot;
    public Vector2 itemRootPosition;

    //About Heal(Point)
    private bool canHeal;
    private Vector2 healPointPosition;

    //About Die
    public bool isDie;

    #endregion

    #region Unity CallBack Functions

    private void Awake()
    {
        CreateStates();
    }

    private void Start()
    {
        isFacingRight = true;
        canDash = true;
        canAirDash = true;
        //TEST : 일단 처음에 스킬 넣어주기
        SkillState.SetSkillID("Sky : Range");
        //SkillState.SetSkillID("Red : Smash");

        GetComponents();
        SetGravityScale(PlayerData.gravityScale);

        //Debug.Log(DataPersistenceManager.instance.gameData.isInitialGame);
        //Initial State when new Game
        //Debug.Log(PlayerManager.instance.isInitialGame);
        if (PlayerManager.instance.isInitialGame)
        {
            transform.position = PlayerManager.instance.lastSavedPosition;
            StateMachine.Initialize(WakeUpState);
        }
        //For Debug
        else if (DataPersistenceManager.instance.disableDataPersistence)
        {
            StateMachine.Initialize(IdleState);
        }
        //else : Load On Bench
        else
        {
            StateMachine.Initialize(LayDownState);
        }
        //Store Jump
        amountOfJumpsLeft = PlayerData.amountOfJumps;
    }

    private void Update()
    {
        Time.timeScale = PlayerData.timeScale;
        //Any State 분기
        //맞았고, 무적 상태가 아니면,
        if (isDie)
        {
            isDie = false;
            StateMachine.ChangeState(DieState);
        }
        else if (getHit && !isInvincibe)
        {
            StateMachine.ChangeState(GetHitState);
            getHit = false;
        }
        else if (getHitByThorn)
        {
            //Change to GetHitByThornState
            getHitByThorn = false;
        }
        CurrentVelocity = RB.velocity;
        //About Attacks
        StateMachine.CurrentState.LogicUpdate();
        ReduceAttackCoolDown();
        ReduceSideAttackResetTime();
        if (isRecoiling) ReduceRecoilTime();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        CheckIfToucingLeftWall();
        CheckIfToucingRightWall();

    }

    private void CreateStates()
    {
        //Generate PlayerStateMachine
        StateMachine = new PlayerStateMachine(this);

        //SubState for PlayerGroundState
        IdleState = new PlayerIdleState(this, StateMachine, PlayerData);
        MoveState = new PlayerMoveState(this, StateMachine, PlayerData);
        StopState = new PlayerStopState(this, StateMachine, PlayerData);
        FlipState = new PlayerFlipState(this, StateMachine, PlayerData);
        RunStopState = new PlayerRunStopState(this, StateMachine, PlayerData);

        //SubState for PlayerAbilityState
        JumpState = new PlayerJumpState(this, StateMachine, PlayerData);
        DashJumpState = new PlayerDashJumpState(this, StateMachine, PlayerData);
        WallJumpState = new PlayerWallJumpState(this, StateMachine, PlayerData);
        BaseAttackState = new PlayerBaseAttackState(this, StateMachine, PlayerData);
        InAirDashState = new PlayerInAirDashState(this, StateMachine, PlayerData);
        RollingState = new PlayerRollingState(this, StateMachine, PlayerData);
        GetHitState = new PlayerGetHitState(this, StateMachine, PlayerData);
        GetHitByThornState = new PlayerGetHitByThornState(this, StateMachine, PlayerData);

        //SkillState
        SkillState = new PlayerSkillState(this, StateMachine, PlayerData);
        SKillState_0_Range = new PlayerSkill_0_Range(this, StateMachine, PlayerData);
        SkillState_1_Smash = new PlayerSkill_1_Smash(this, StateMachine, PlayerData);

        //SaveStates
        SaveState = new PlayerSaveState(this, StateMachine, PlayerData);
        LayDownState = new PlayerLayDownState(this, StateMachine, PlayerData);

        //TalkState
        TalkState = new PlayerTalkState(this, StateMachine, PlayerData);

        //ItemRootState
        RootState = new PlayerItemRootState(this, StateMachine, PlayerData);

        //HealState
        HealState = new PlayerHealState(this, StateMachine, PlayerData);

        DieState = new PlayerDieState(this, StateMachine, PlayerData);

        //SubState for PlayerGroundDashState
        DashState = new PlayerDashState(this, StateMachine, PlayerData);
        RunState = new PlayerRunState(this, StateMachine, PlayerData);

        //StandAlone State
        InAirState = new PlayerInAirState(this, StateMachine, PlayerData);
        LandState = new PlayerLandState(this, StateMachine, PlayerData);

        //Substate for PlayerToucingWallState
        WallSlideState = new PlayerWallSlideState(this, StateMachine, PlayerData);

        //Player BehaviorState
        WakeUpState = new PlayerWakeUpState(this, StateMachine, PlayerData);
    }

    private void GetComponents()
    {
        if (!TryGetComponent<Animator>(out Animator)) Debug.LogError("Animator Missing");
        if (Animator == null)
        {
            Debug.Log("umm");
        }
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        if (!TryGetComponent<SpriteRenderer>(out SpriteRenderer)) Debug.LogError("SpriteRender Missing");
        PlayerMaterial = SpriteRenderer.material;
        strongTintFadeID = Shader.PropertyToID("_StrongTintFade");
    }

    #endregion

    #region Move using Forces

    public void MoveX(float acclerationInAir, float decelerationInAir)
    {
        if (!isRecoiling)
        {
            float targetSpeed = InputHandler.NormInputX * PlayerData.maxMoveSpeed;
            float accelRate;
            if (CheckIfGrounded())
            {
                if (Mathf.Abs(targetSpeed) > 0.01f)
                    accelRate = PlayerData.accelerationAmount;
                else
                    accelRate = PlayerData.decelerationAmount;
            }
            else
            {
                if (Mathf.Abs(targetSpeed) > 0.01f)
                    accelRate = PlayerData.accelerationAmount * acclerationInAir;
                else
                    accelRate = PlayerData.decelerationAmount * decelerationInAir;
            }
            float speedDif = targetSpeed - CurrentVelocity.x;
            float movement = speedDif * accelRate;
            RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
    }

    public void DashMoveX(float acclerationInAir, float decelerationInAir)
    {
        Debug.Log("umm");
        float targetSpeed = InputHandler.NormInputX * PlayerData.maxMoveSpeed;
        float accelRate;
        if (Mathf.Abs(targetSpeed) > 0.01f)
            accelRate = PlayerData.accelerationAmount * acclerationInAir;
        else
            accelRate = PlayerData.decelerationAmount * decelerationInAir;
        float speedDif = targetSpeed - CurrentVelocity.x;
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void AddWallJumpForce(int dir)
    {
        Vector2 force = new Vector2(PlayerData.wallJumpForce.x, PlayerData.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall
        RB.AddForce(force, ForceMode2D.Impulse);
    }

    public void AddInAirDashForce(int dir)
    {
        Vector2 force = new Vector2(PlayerData.inAirDashForce.x, PlayerData.inAirDashForce.y);
        force.x *= dir;
        RB.AddForce(force, ForceMode2D.Impulse);
    }

    public void HitStop(float time, float scale)
    {
        Debug.Log(scale);
        Time.timeScale = scale;
        StartCoroutine(StartHitStop(time));
    }

    IEnumerator StartHitStop(float time)
    {
        WaitForSecondsRealtime wfsR = new WaitForSecondsRealtime(time);
        yield return wfsR;
        Time.timeScale = 1f;

    }

    #region Recoil

    public void StartXRecoil(float xForce, float recoilTime)
    {
        RB.velocity = new Vector2(0, RB.velocity.y);
        recoilTimeCounter = recoilTime;
        isRecoiling = true;
        float force = (isFacingRight) ? -xForce : xForce;
        RB.AddForce(force * Vector2.right, ForceMode2D.Impulse);
    }
    public void StartYRecoil(float yForce)
    {
        RB.velocity = new Vector2(RB.velocity.x, yForce);
        //RB.AddForce(yForce * Vector2.up, ForceMode2D.Impulse);
    }

    #endregion

    public void PlayAnimation(string name)
    {
        Animator.Play(name);
    }

    private void ReduceRecoilTime()
    {
        //if Player goes opposite dir, end recoiling
        float xInput = InputHandler.NormInputX;
        if ((isFacingRight && xInput == -1) || (!isFacingRight && xInput == 1))
        {
            isRecoiling = false;
        }
        recoilTimeCounter -= Time.deltaTime;
        if (recoilTimeCounter < 0)
        {
            isRecoiling = false;
        }
    }

    public void InAirDashDeceleration()
    {
        float targetSpeed = (isFacingRight) ? PlayerData.maxMoveSpeed : -PlayerData.maxMoveSpeed;
        float speedDif = targetSpeed - CurrentVelocity.x;
        float movement = speedDif * PlayerData.inAirDashDeceleration;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void AddWallSlideForce()
    {
        float targetSpeed = -PlayerData.wallSlideSpeed;

        float accelRate;

        // 현재 y가속도 < 목표 속도
        if (CurrentVelocity.y <= targetSpeed) accelRate = 30;
        else accelRate = PlayerData.wallSlideAccelerationAmount;
        float speedDif = targetSpeed - CurrentVelocity.y;
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.up, ForceMode2D.Force);
    }
    #endregion

    #region Checks

    public bool CheckIfGrounded()
    {
        return isGrounded;
    }

    public void SetIsTouchingRightWall(bool temp) => isToucingRightWall = temp;

    public void SetIsTouchingLeftWall(bool temp) => isToucingLeftWall = temp;

    public bool CheckIfToucingRightWall()
    {
        /* isToucingRightWall =
        ((Physics2D.OverlapBox(frontWallCheck.position, PlayerData.wallCheckSize, 0, PlayerData.whatIsGround) && isFacingRight)
        || (Physics2D.OverlapBox(backWallCheck.position, PlayerData.wallCheckSize, 0, PlayerData.whatIsGround) && !isFacingRight));
        return isToucingRightWall; */
        return isToucingRightWall;
    }

    public bool CheckIfToucingLeftWall()
    {
        /* isToucingLeftWall =
        ((Physics2D.OverlapBox(frontWallCheck.position, PlayerData.wallCheckSize, 0, PlayerData.whatIsGround) && !isFacingRight)
        || (Physics2D.OverlapBox(backWallCheck.position, PlayerData.wallCheckSize, 0, PlayerData.whatIsGround) && isFacingRight));
        return isToucingLeftWall; */
        return isToucingLeftWall;
    }

    public bool CheckIfToucingWall()
    {
        if (PlayerManager.instance.canWallJump)
        {
            return isToucingRightWall || isToucingLeftWall;
        }
        else
        {
            return false;
        }
    }

    public bool CheckCanGroundDash()
    {
        return Physics2D.OverlapBox(dashCheck.position, PlayerData.dashCheckSize, 0, PlayerData.whatIsGround);
    }

    public bool CheckIfShouldFlip(int xInput)
    {
        if ((xInput == -1 && isFacingRight == true) || (xInput == 1 && isFacingRight == false))
        {
            return true;
        }
        return false;
    }

    public Collider2D[] CheckAttackHitted(string temp)
    {
        Collider2D[] col;
        if (temp.Equals("Side"))
            col = Physics2D.OverlapBoxAll(BaseSideAttackPos.position, PlayerData.baseSideAttackRange, 0, PlayerData.checkIsEnemies);
        else if (temp.Equals("Up"))
            col = Physics2D.OverlapBoxAll(BaseUpAttackPos.position, PlayerData.baseUpAttackRange, 0, PlayerData.checkIsEnemies);
        else if (temp.Equals("Down"))
            col = Physics2D.OverlapBoxAll(BaseDownAttackPos.position, PlayerData.baseDownAttackRange, 0, PlayerData.checkIsEnemies);
        else col = null;
        return col;
    }
    public Collider2D[] CheckGroundHitted(string temp)
    {
        Collider2D[] col;
        if (temp.Equals("Side"))
            col = Physics2D.OverlapBoxAll(BaseSideAttackPos.position, PlayerData.baseSideAttackRange, 0, LayerMask.NameToLayer("Colliders"));
        else if (temp.Equals("Up"))
            col = Physics2D.OverlapBoxAll(BaseUpAttackPos.position, PlayerData.baseUpAttackRange, 0, LayerMask.NameToLayer("Colliders"));
        else if (temp.Equals("Down"))
            col = Physics2D.OverlapBoxAll(BaseDownAttackPos.position, PlayerData.baseDownAttackRange, 0, LayerMask.NameToLayer("Colliders"));
        else col = null;
        return col;
    }

    public bool CheckIfGroundAttackResetTimeEnded() => (groundSideAttackResetTimeCounter <= 0);


    #region Save

    public void SetCanSave(bool temp) => canSave = temp;
    public bool CanSave() => canSave;
    public void SetSavePointPosition(Vector2 position) => savePointPosition = position;
    public Vector2 GetSavePointPosition() => savePointPosition;

    #endregion

    #region Heal(Point)

    public void SetCanHeal(bool temp) => canHeal = temp;
    public bool CanHeal() => canHeal;
    public void SetHealPointPosition(Vector2 position) => healPointPosition = position;
    public Vector2 GetHealPointPosition() => healPointPosition;

    #endregion

    #endregion

    #region Setter

    public void SetGravityScale(float scale) => RB.gravityScale = scale;

    public void SetIsGrounded(bool temp) => isGrounded = temp;

    public void ResetAmoutOfJumpLeft() => amountOfJumpsLeft = PlayerData.amountOfJumps;

    public bool CanJump() => (amountOfJumpsLeft > 0) ? true : false;

    public void DecreaseAmoutOfJumpsLeft() => amountOfJumpsLeft--;

    #endregion

    #region Attack

    public void SetAttackCoolDown(float cooldown) => attackCoolDownCounter = cooldown;

    private void ReduceAttackCoolDown() => attackCoolDownCounter -= Time.deltaTime;

    public bool CanAttack() => (attackCoolDownCounter > 0) ? false : true;

    //InAirDash
    public bool CanAirDash()
    {
        if (PlayerManager.instance.canInAirDash)
        {
            return canAirDash;
        }
        else
        {
            return false;
        }
    }

    public void ResetCanAirDash() => canAirDash = true;

    public void UseAirDash() => canAirDash = false;

    public void AirDashCoolDown()
    {
        canAirDash = false;
        StartCoroutine(AirDashCoroutine());
    }

    IEnumerator AirDashCoroutine()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.14f);
        yield return wfs;
        canAirDash = true;
    }

    //Ground Dash
    public bool CanDash()
    {
        if (PlayerManager.instance.canDash)
        {
            return canDash;
        }
        else
        {
            return false;
        }
    }

    public void UseDash() => canDash = false;

    public void SetDashCoolDown() => StartCoroutine(StartDashCoolDown());

    //Ground Dash CoolDown
    IEnumerator StartDashCoolDown()
    {
        WaitForSeconds wfs = new WaitForSeconds(PlayerData.dashCoolDown);
        yield return wfs;
        canDash = true;
    }

    public void SetSideAttackResetTime() => groundSideAttackResetTimeCounter = PlayerData.SideAttackResetTime;

    private void ReduceSideAttackResetTime() => groundSideAttackResetTimeCounter -= Time.deltaTime;

    #endregion

    #region Flip & ChangeDir

    //just flip
    public void Flip()
    {
        isFacingRight = (isFacingRight) ? false : true;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    #endregion

    #region Other Functions

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationTrigger2()
    {
        StateMachine.CurrentState.AnimationTrigger2();
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    private void PlaySound(string temp)
    {
        PlayerSounds.instance.PlaySound(temp);
    }

    public void ClampFallSpeed()
    {
        workSpace.Set(CurrentVelocity.x, Mathf.Max(CurrentVelocity.y, -PlayerData.maxFallSpeed));
        RB.velocity = workSpace;
    }

    #endregion

    public void PlayerHit(int damage)
    {
        if (!isInvincibe)
        {
            PlayerManager.instance.PlayerDamaged(damage);
            if (PlayerManager.instance.currentHP <= 0)
            {
                isDie = true;
                isInvincibe = true;
            }
            else
            {
                getHit = true;
            }
        }
    }

    public void PlayerHitByThorn(int damage, Vector2 teleportPos, bool dirRight)
    {
        PlayerManager.instance.PlayerDamaged(damage);
        if (PlayerManager.instance.currentHP <= 0)
        {
            isDie = true;
            isInvincibe = true;
        }
        else
        {
            getHitByThorn = true;
        }
        
        GetHitByThornState.SetTeleportPos(teleportPos);
        GetHitByThornState.SetPlayerDirAfterTP(dirRight);
        StateMachine.ChangeState(GetHitByThornState);
    }

    //깜빡이는 무적 시간 적용 시간
    public void SetisInvincibe(bool temp)
    {
        isInvincibe = true;
        StartCoroutine(StartFlicker(PlayerData.hitInvincibeTime));
    }



    //깜빡이는 효과 구현
    IEnumerator StartFlicker(float time)
    {
        float fadeValue = 0.6f;
        int repeatCount = PlayerData.hitFlickerRepeatFrequency;
        WaitForSeconds wfs = new WaitForSeconds(time / repeatCount);
        PlayerMaterial.SetFloat(strongTintFadeID, fadeValue);
        yield return wfs;
        for (int i = 0; i < repeatCount; i++)
        {
            if (fadeValue <= 0)
            {
                fadeValue = 0.6f;
            }
            else
            {
                fadeValue = 0f;
            }
            PlayerMaterial.SetFloat(strongTintFadeID, fadeValue);
            yield return wfs;
        }
        fadeValue = 0f;
        PlayerMaterial.SetFloat(strongTintFadeID, fadeValue);
        isInvincibe = false;
    }

    public void FreezeTime(float time)
    {
        Time.timeScale = 0f;
        StartCoroutine(TimeFreeze(time));
    }

    IEnumerator TimeFreeze(float time)
    {
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(time);
        yield return wfsr;
        Time.timeScale = 1f;
    }


    public void CreateRipple()
    {
        //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        //RippleEffect.Emit(Camera.main.WorldToViewportPoint(transform.position));
    }

    private void OnDrawGizmos()
    {
        //wallcheck
        /* Gizmos.color = Color.red;
        Gizmos.DrawWireCube(frontWallCheck.position, PlayerData.wallCheckSize);
        Gizmos.DrawWireCube(backWallCheck.position, PlayerData.wallCheckSize); */
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(BaseSideAttackPos.position, PlayerData.baseSideAttackRange);
        Gizmos.DrawWireCube(BaseUpAttackPos.position, PlayerData.baseUpAttackRange);
        Gizmos.DrawWireCube(BaseDownAttackPos.position, PlayerData.baseDownAttackRange);
        /* Gizmos.DrawWireCube(dashCheck.position, PlayerData.dashCheckSize); */
    }
}
