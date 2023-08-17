using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealState : PlayerState
{
    /* private int yInput;
    private Vector2 healPointPosition;
    private Vector2 velocity = Vector2.zero;

    private bool moveDone;
    private float moveTime;
    private float moveTimeCounter;

    //private bool healTimeDone;
    private float healTimeCounter;

    private float flipIgnoreRange = 0.05f; */

    private float healTimeCounter;

    GameObject obj;

    public PlayerHealState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        /* Player.SetGravityScale(0);
        healPointPosition = Player.GetHealPointPosition();

        if (Player.transform.position.x < healPointPosition.x - flipIgnoreRange)
        {
            if (!Player.isFacingRight) Player.Flip();
        }

        if (Player.transform.position.x > healPointPosition.x + flipIgnoreRange)
        {
            if (Player.isFacingRight) Player.Flip();
        }

        moveDone = false;
        //healTimeDone = false;



        moveTime = 0.23f;
        healTimeCounter = 1.34f;
        

        moveTimeCounter = moveTime; */

        healTimeCounter = 1.57f;
        Player.PlayAnimation("Heal_Pre");
        Player.RB.velocity = new Vector2(0, 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        healTimeCounter -= Time.deltaTime;
        if (healTimeCounter < 0)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        /* yInput = Player.InputHandler.NormInputY;

        if (moveTimeCounter >= 0) moveTimeCounter -= Time.deltaTime;
        else moveDone = true;

        if (moveDone == true)
        {
            Player.PlayAnimation("Heal_Middle");
            if (healTimeCounter >= 0) healTimeCounter -= Time.deltaTime;
            //Heal Done
            else StateMachine.ChangeState(Player.IdleState);
        }

        MoveTowardsHealPoint(healPointPosition); */
    }
    public override void Exit()
    {
        base.Exit();
        if(obj != null) PlayerDetachedEffectPool.instance.ReturnToPool(obj);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        obj = PlayerDetachedEffectPool.instance.GetFromPool("HealPointEffect", Player.transform.position, Quaternion.identity);
    }

    public override void AnimationTrigger2()
    {
        base.AnimationTrigger2();
        PlayerManager.instance.PlayerHeal(1);
    }


    /* private void MoveTowardsHealPoint(Vector2 position)
    {
        Player.transform.position = Vector2.SmoothDamp(Player.transform.position, position, ref velocity, 0.1f);
    } */
}
