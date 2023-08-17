using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirDashState : PlayerAbilityState
{
    private int dir;
    private float inAirDashTimeCounter;

    private float cantWallSlideCounter;

    private bool isToucingWall;
    public PlayerInAirDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
        isToucingWall = Player.CheckIfToucingWall();
    }
    public override void Enter()
    {
        base.Enter();
        dir = (Player.isFacingRight) ? 1 : -1;
        inAirDashTimeCounter = PlayerData.inAirDashTime;
        Player.SetGravityScale(0);
        Player.RB.velocity = new Vector2(PlayerData.inAirDashForce.x * dir, 0);

        Player.PlayAnimation("InAirDash");
        //Debug.Log(Player.RB.velocity.x);
        if (dir == 1)
        {
            PlayerDetachedEffectPool.instance.GetFromPool("InAirDashEffect", Quaternion.Euler(0, 180, 0));
        }
        else
        {
            PlayerDetachedEffectPool.instance.GetFromPool("InAirDashEffect", Quaternion.identity);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Player.SetGravityScale(PlayerData.gravityScale * PlayerData.fastFallGravityMultiplier);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        InAirDashDeceleration();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(cantWallSlideCounter > 0) cantWallSlideCounter -= Time.deltaTime;
        inAirDashTimeCounter -= Time.deltaTime;
        if (isToucingWall && cantWallSlideCounter <= 0)
        {
            StateMachine.ChangeState(Player.WallSlideState);
        }
        else if (inAirDashTimeCounter < 0)
        {
            Player.StateMachine.ChangeState(Player.RollingState);
        }
    }

    private void InAirDashDeceleration()
    {
        float targetSpeed = (dir == 1) ? PlayerData.runSpeed : -PlayerData.runSpeed;
        float speedDif = targetSpeed - Player.CurrentVelocity.x;
        float movement = speedDif * PlayerData.dashDeceleration;
        Player.RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    public void SetCantWallSlide()
    {
        cantWallSlideCounter = 0.1f;
    }
}
