using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunStopState : PlayerState
{
    private int xInput;
    private bool isGrounded;
    private float runStopDownTimeCounter;
    private bool jumpInput;
    private bool attackInput;
    private int yInput;

    public PlayerRunStopState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData) : base(player, playerStateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = Player.InputHandler.NormInputX;
        if (xInput != 0)
        {
            Player.MoveState.SetAnimation("Run_Stop_Up_Move");
            StateMachine.ChangeState(Player.MoveState);
        }

        if (xInput == 0) Player.PlayAnimation("Run_Stop_Down"); 
        PlayerParticleManager.ParticleManager.PlayParticle("0.Move_Dust");
        runStopDownTimeCounter = PlayerData.runStopDownTime;
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isGrounded = Player.CheckIfGrounded();
    }

    public override void Exit()
    {
        base.Exit();
        PlayerParticleManager.ParticleManager.StopParticle("0.Move_Dust");
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = Player.InputHandler.NormInputX;
        runStopDownTimeCounter -= Time.deltaTime;
        jumpInput = Player.InputHandler.JumpInput;
        attackInput = Player.InputHandler.AttackInput;
        yInput = Player.InputHandler.NormInputY;

        if (jumpInput && Player.CanJump())
        {
            Player.InputHandler.UseJumpInput();
            StateMachine.ChangeState(Player.JumpState);
        }
        else if (xInput != 0)
        {
            Player.MoveState.SetAnimation("Run_Stop_Up_Move");
            StateMachine.ChangeState(Player.MoveState);
        }
        else if (!isGrounded)
        {
            Player.DecreaseAmoutOfJumpsLeft();
            StateMachine.ChangeState(Player.InAirState);
        }
        else if (attackInput && Player.CanAttack() && yInput != -1)
        {
            Player.InputHandler.UseAttackInput();
            Player.BaseAttackState.SetYInput(yInput);
            StateMachine.ChangeState(Player.BaseAttackState);
        }
        //Save
        else if (Player.CanSave() && yInput == 1)
        {
            StateMachine.ChangeState(Player.SaveState);
        }
        else if (runStopDownTimeCounter < 0)
        {
            if (!isGrounded)
            {
                Player.DecreaseAmoutOfJumpsLeft();
                StateMachine.ChangeState(Player.InAirState);
            }
            else
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        RunStopDeceleration();
    }

    private void RunStopDeceleration()
    {
        float speedDif = 0 - Player.CurrentVelocity.x;
        float movement = speedDif * PlayerData.runDeceleration;
        Player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
