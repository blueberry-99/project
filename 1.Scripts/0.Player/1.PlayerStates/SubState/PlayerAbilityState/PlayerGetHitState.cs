using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitState : PlayerAbilityState
{
    private float hitTimeCounter;

    public PlayerGetHitState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.SetisInvincibe(true);
        Player.SetGravityScale(0);

        hitTimeCounter = PlayerData.getHitTime;
        Player.PlayAnimation("GetHit");
        float angle = (Player.isFacingRight) ? 0 : 180;
        PlayerDetachedEffectPool.instance.GetFromPool("PlayerHitEffect", Quaternion.Euler(0, angle, -30));
        Player.FreezeTime(PlayerData.hitFreezeTime);
        float xForce = (Player.isFacingRight) ? -PlayerData.knockBackVelocity.x : PlayerData.knockBackVelocity.x;
        Player.RB.velocity = new Vector2(xForce, PlayerData.knockBackVelocity.y);

        PlayerSounds.instance.PlaySound("PlayerHitSound");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (hitTimeCounter >= 0) hitTimeCounter -= Time.deltaTime;
        else isAbilityDone = true;

    }

    public override void Exit()
    {
        base.Exit();
        Player.SetGravityScale(PlayerData.gravityScale);
        Player.DecreaseAmoutOfJumpsLeft();
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
