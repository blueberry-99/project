using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerState
{
    private bool isGrounded;
    private bool isToucingWall;
    private bool AttackInput;
    private bool SkillInput;
    private int xInput;
    private int yInput;

    private bool facingRight;
    private bool canMove;

    private int dir;

    public PlayerRollingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
        Player.PlayAnimation("Roll");
        facingRight = Player.isFacingRight;
        dir = (facingRight) ? 1 : -1;
        Player.RB.velocity = new Vector2(PlayerData.rollSpeed * dir, Player.RB.velocity.y);

        canMove = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        AttackInput = Player.InputHandler.AttackInput;
        xInput = Player.InputHandler.NormInputX;
        yInput = Player.InputHandler.NormInputY;
        SkillInput = Player.InputHandler.SkillInput;
        
        if (Player.CheckIfShouldFlip(xInput)) Player.Flip();

        //if Player does opposite dir, start move
        if (facingRight && xInput == -1) canMove = true;
        if (!facingRight && xInput == 1) canMove = true;
        //Player.ClampFallSpeed();
        //Land
        if (isGrounded && Player.CurrentVelocity.y < 0.01f)
        {
            StateMachine.ChangeState(Player.LandState);
        }
        //Wall
        else if (isToucingWall)
        {
            StateMachine.ChangeState(Player.WallSlideState);
        }
        //Attack
        else if (AttackInput && Player.CanAttack())
        {
            Player.InputHandler.UseAttackInput();
            Player.BaseAttackState.SetYInput(yInput);
            StateMachine.ChangeState(Player.BaseAttackState);
        }
        //Skill
        else if (SkillInput && PlayerManager.instance.IsManaFull() && PlayerManager.instance.canSkill)
        {
            PlayerManager.instance.UseMana();
            Player.InputHandler.UseSkillInput();
            Player.SkillState.SetInput(xInput, yInput);
            StateMachine.ChangeState(Player.SkillState);
        }
        //TODO : Rolltime으로 바꿀 것
        else if (isAnimationFinished)
        {
            Player.InAirState.SetCustomInAirAccel
            (PlayerData.inAirDashAccelerationInAir, PlayerData.inAirDashDecelerationInAir, PlayerData.inAirDashCustomAccelTime);
            StateMachine.ChangeState(Player.InAirState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (canMove) Player.MoveX(PlayerData.rollChangeDirAcceleration, PlayerData.inAirDashDecelerationInAir);
    }
}
