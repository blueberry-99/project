using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitByThornState : PlayerAbilityState
{
    //FadeOut
    //private float fadeOutTimeCounter;
    private bool isFaded;

    //Teleport
    private float teleportTimeCounter;
    private bool isTeleported;

    //FadeIn
    private float fadeOutTimeCounter;

    private float hitTimeCounter;

    private Vector2 teleportPos;

    private int dirAfterTP;

    public PlayerGetHitByThornState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.isInvincibe = true;
        Player.SetGravityScale(0);
        FadeManager.fadeManager.FadeOut();

        isFaded = false;
        fadeOutTimeCounter = 0.33f;

        isTeleported = false;
        teleportTimeCounter = 0.5f;


        hitTimeCounter = PlayerData.getHitTime;
        Player.PlayAnimation("GetHitByThorn");
        float angle = (Player.isFacingRight) ? 0 : 180;
        PlayerDetachedEffectPool.instance.GetFromPool("PlayerHitEffect", Quaternion.Euler(0, angle, -30));
        Player.FreezeTime(PlayerData.hitFreezeTime);
        float xForce = (Player.isFacingRight) ? -1 : 1;
        Player.RB.velocity = new Vector2(xForce, 0.5f);

        PlayerSounds.instance.PlaySound("PlayerHitSound");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isFaded)
        {
            if (fadeOutTimeCounter >= 0)
            {
                fadeOutTimeCounter -= Time.deltaTime;
            }
            else
            {
                isFaded = true;
                //Teleport Here;
                Player.SetGravityScale(PlayerData.gravityScale);
                Player.RB.velocity = Vector2.zero;
                Player.transform.position = teleportPos;

                //Flip
                if (dirAfterTP == 1 && Player.isFacingRight == false) Player.Flip();
                else if (dirAfterTP == -1 && Player.isFacingRight == true) Player.Flip();
            }
        }
        else if (!isTeleported)
        {
            if (teleportTimeCounter >= 0)
            {
                teleportTimeCounter -= Time.deltaTime;
            }
            else
            {
                isTeleported = true;
                FadeManager.fadeManager.FadeIn();
                //FadeIn Here
            }
        }
        else
        {
            Player.WakeUpState.SetIsQuickWakeUp();
            StateMachine.ChangeState(Player.WakeUpState);
        }
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

    public void SetTeleportPos(Vector2 pos)
    {
        teleportPos = pos;
    }

    public void SetPlayerDirAfterTP(bool isFacingRight)
    {
        dirAfterTP = (isFacingRight) ? 1 : -1;
    }
}
