using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToucingWallState : PlayerState
{
    private bool isGrounded;
    private bool isToucingWall;
    public int xInput;

    
    public PlayerToucingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = Player.InputHandler.NormInputX;

        if (!isToucingWall)
        {
            StateMachine.ChangeState(Player.InAirState);
        }
        else if (isGrounded)
        {
            StateMachine.ChangeState(Player.LandState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Player.MoveX(PlayerData.accelerationInAir, PlayerData.decelerationInAir);
    }
}
