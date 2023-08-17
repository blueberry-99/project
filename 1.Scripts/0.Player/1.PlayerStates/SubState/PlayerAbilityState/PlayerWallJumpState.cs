using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDir;

    private bool isToucingWall;
    private bool jumpInput;

    private bool canMove;
    private float wallJumpTimeCounter;
    private float wallJumpInputStopTimeCounter;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        canMove = false;
        wallJumpTimeCounter = PlayerData.wallJumpTime;
        wallJumpInputStopTimeCounter = PlayerData.wallJumpInputStopTime;

        Player.RB.velocity = new Vector2(0, 0);
        Player.AddWallJumpForce(wallJumpDir);

        Player.PlayAnimation("WallJump");
        Quaternion rotation = Player.isFacingRight ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        PlayerDetachedEffectPool.instance.GetFromPool("WallJumpEffect", rotation);

        Player.Flip();
        Player.SetGravityScale(PlayerData.wallJumpGravityScale);
        Player.DecreaseAmoutOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        jumpInput = Player.InputHandler.JumpInput;

        wallJumpTimeCounter -= Time.deltaTime;
        wallJumpInputStopTimeCounter -= Time.deltaTime;

        //wallSlide
        if (wallJumpInputStopTimeCounter < 0 && isToucingWall) StateMachine.ChangeState(Player.WallSlideState);
        else if (Player.RB.velocity.y < 0) isAbilityDone = true;
        else if (wallJumpTimeCounter < 0) isAbilityDone = true;

        if (wallJumpInputStopTimeCounter < 0)
        {
            canMove = true;
            if (Player.CheckIfShouldFlip(xInput))
            {
                Player.Flip();
                Player.PlayAnimation("Rise_Start");
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (canMove) Player.MoveX(PlayerData.wallJumpAccelerationInAir, PlayerData.wallJumpDecelerationInAir);
    }

    public override void Exit()
    {
        base.Exit();
        if (xInput == 0)
        {
            Player.InAirState.SetCustomInAirAccel
            (PlayerData.wallJumpAccelerationInAir, PlayerData.wallJumpDecelerationInAir, PlayerData.wallJumpCustomAccelTime);
        }
        //인풋이 없으면, 이렇게 하고, 인풋이 있으면, 그냥 Inairstate customaccel 없게..!

    }

    public override void DoCheck()
    {
        base.DoCheck();
        isToucingWall = Player.CheckIfToucingWall();
    }

    public void SetWallJumpDir(int dir)
    {
        wallJumpDir = dir;
    }

}
