using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    //Input
    private int xInput;
    private int yInput;
    private bool JumpInput;
    private bool JumpInputStop;
    private bool AttackInput;
    private bool DashInput;
    private bool DashInputHold;
    private bool SkillInput;

    //Checks
    private bool isGrounded;
    private bool isToucingWall;

    public bool coyoteTime;
    private float coyoteTimeCounter;

    //for custom acceleration, deceleration
    private float customTime;
    private float customTimeCounter;
    private float accelerationInAir = 1;
    private float decelerationInAir = 0.3f;

    //for define Rise & Fall
    private bool isRising;
    //private bool isFalling;

    private bool isDashJumping;
    //for Deceleration
    private int dashJumpDir;

    private bool playOtherAnimation;
    private string otherAnimation;
    private float otherAnimationTimeCounter;

    private float customGravityScale;
    private float customGravityTimeCounter;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isGrounded = Player.CheckIfGrounded();
        isToucingWall = Player.CheckIfToucingWall();
    }

    public override void Enter()
    {
        base.Enter();
        
        xInput = Player.InputHandler.NormInputX;
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();
        //Animation
        if (playOtherAnimation)
        {
            Player.PlayAnimation(otherAnimation);
        }
        else
        {
            isRising = (Player.CurrentVelocity.y >= 0) ? true : false;
            string str = (Player.CurrentVelocity.y >= 0) ? "Rise_Start" : "Fall_Start";
            Player.PlayAnimation(str);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isRising = false;
        isDashJumping = false;
        playOtherAnimation = false;

        accelerationInAir = PlayerData.accelerationInAir; //original acceleration in Air
        decelerationInAir = PlayerData.decelerationInAir; //original deceleration in Air
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        GetInputs();

        if (otherAnimationTimeCounter < 0) playOtherAnimation = false;

        if (isDashJumping)
        {
            if (dashJumpDir == 1 && xInput == -1 || dashJumpDir == -1 && xInput == 1) isDashJumping = false;
        }

        if (!playOtherAnimation)
        {
            if (isRising)
            {
                if (Player.CurrentVelocity.y < 0)
                {
                    Player.PlayAnimation("Fall_Start");
                    isRising = false;
                }
            }
            else
            {
                if (Player.CurrentVelocity.y >= 0)
                {
                    Player.PlayAnimation("Rise_Start");
                    isRising = true;
                }
            }
        }



        //On Air Flip_ Flip without changing to Flip State 
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();
        CheckCoyoteTime();

        //for WallJump Custom In Air Acceleration
        CheckCustomTime();

        //TODO
        Player.ClampFallSpeed();
        ChangeGravity();

        if (DashInputHold && isGrounded && Player.CanDash())
        {
            Player.UseDash();
            StateMachine.ChangeState(Player.DashState);
        }
        else if (isGrounded && Player.CurrentVelocity.y < 0.01f)
        {
            StateMachine.ChangeState(Player.LandState);
        }
        //coyoteJump & DoubleJump
        else if (JumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            StateMachine.ChangeState(Player.JumpState);
        }
        //wallSlide
        else if (isToucingWall && Player.CurrentVelocity.y <= 1)
        {
            StateMachine.ChangeState(Player.WallSlideState);
        }
        //Attacks
        else if (AttackInput && Player.CanAttack())
        {
            Player.InputHandler.UseAttackInput();
            Player.BaseAttackState.SetYInput(yInput);
            StateMachine.ChangeState(Player.BaseAttackState);
        }
        //Skills
        else if (SkillInput && PlayerManager.instance.IsManaFull() && PlayerManager.instance.canSkill)
        {
            PlayerManager.instance.UseMana();
            Player.InputHandler.UseSkillInput();
            Player.SkillState.SetInput(xInput, yInput);
            StateMachine.ChangeState(Player.SkillState);
        }
        
        //InAirDash & Dash
        else if (DashInput && Player.CanAirDash() && !Player.CheckCanGroundDash())
        {
            Player.UseAirDash();
            Player.InputHandler.UseDashInput();
            StateMachine.ChangeState(Player.InAirDashState);
        }
        else
        {

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isDashJumping)
        {
            DashJumpDeceleration();
        }
        else Player.MoveX(accelerationInAir, decelerationInAir);

    }

    private void ChangeGravity()
    {
        customGravityTimeCounter -= Time.deltaTime;
        if (customGravityTimeCounter > 0)
        {
            Player.SetGravityScale(customGravityScale);
        }
        else if (JumpInputStop)
        {
            Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
        }
    }

    #region CoyoteTime

    public void StartCoyoteTime()
    {
        coyoteTime = true;
        coyoteTimeCounter = PlayerData.coyoteTime;
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime)
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (coyoteTimeCounter < 0)
            {
                coyoteTime = false;
                Player.DecreaseAmoutOfJumpsLeft();
            }
        }
    }

    #endregion

    public void SetOtherAnimation(string str, float time)
    {
        playOtherAnimation = true;
        otherAnimation = str;
        otherAnimationTimeCounter = time;
    }

    public void SetIsDashJumping(int dir)
    {
        dashJumpDir = dir;
        isDashJumping = true;
    }

    public void SetCustomGravity(float scale, float time)
    {
        customGravityScale = scale;
        customGravityTimeCounter = time;
    }

    public void SetCustomInAirAccel(float accel, float decel, float time)
    {
        customTime = time;
        accelerationInAir = accel;
        decelerationInAir = decel;
    }

    public void CheckCustomTime()
    {
        customTime -= Time.deltaTime;
        if (customTime <= 0 || xInput != 0)
        {
            accelerationInAir = PlayerData.accelerationInAir; //original acceleration in Air
            decelerationInAir = PlayerData.decelerationInAir; //original deceleration in Air
        }
    }

    public void GetInputs()
    {
        xInput = Player.InputHandler.NormInputX;
        yInput = Player.InputHandler.NormInputY;
        JumpInput = Player.InputHandler.JumpInput;
        JumpInputStop = Player.InputHandler.JumpInputStop;
        AttackInput = Player.InputHandler.AttackInput;
        DashInput = Player.InputHandler.DashInput;
        DashInputHold = Player.InputHandler.DashInputHold;
        SkillInput = Player.InputHandler.SkillInput;
    }

    private void DashJumpDeceleration()
    {
        float targetSpeed = (dashJumpDir == 1) ? PlayerData.maxMoveSpeed : -PlayerData.maxMoveSpeed;
        float speedDif = targetSpeed - Player.CurrentVelocity.x;
        float movement = speedDif * PlayerData.dashJumpDecel;
        Player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

}
