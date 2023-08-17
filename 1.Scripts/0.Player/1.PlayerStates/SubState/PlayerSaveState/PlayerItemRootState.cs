using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemRootState : PlayerState
{
    private Item item;
    //private bool isArrived;

    private Vector2 itemPosition;

    private float itemRootTimeCounter;

    public PlayerItemRootState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //isArrived = false;
        Player.RB.velocity = Vector2.zero;
        itemRootTimeCounter = 1f;
        Player.PlayAnimation("ItemRoot");
        //아이템 방향으로 플레이어 회전, Set Velocity로 아이템까지 움직임 설정
        /* if (Mathf.Abs(itemPosition.x - Player.transform.position.x) <= 0.2f)
        {

        }
        else if (itemPosition.x < Player.transform.position.x)
        {
            Player.PlayAnimation("Bend");
            if (Player.isFacingRight) Player.Flip();
            Player.RB.velocity = new Vector2(-PlayerData.maxMoveSpeed, 0);
        }
        else
        {
            Player.PlayAnimation("Bend");
            if (!Player.isFacingRight) Player.Flip();
            Player.RB.velocity = new Vector2(PlayerData.maxMoveSpeed, 0);
        } */
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        /* if (!isArrived)
        {
            if (Mathf.Abs(itemPosition.x - Player.transform.position.x) <= 0.2f)
            {
                Player.RB.velocity = Vector2.zero;
                isArrived = true;
                Player.PlayAnimation("ItemRoot_Down");
            }
        }

        if (isArrived)
        {
            itemRootTimeCounter -= Time.deltaTime;
            if (itemRootTimeCounter < 0)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        } */
/*         itemRootTimeCounter -= Time.deltaTime;

        if (itemRootTimeCounter < 0)
        {
            
        } */
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        item.PlayerGetItem();
    }

    public override void AnimationTrigger2()
    {
        base.AnimationTrigger2();
        StateMachine.ChangeState(Player.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void SetItemPosition(Vector2 pos) => itemPosition = pos;

    public void SetItem(Item item)
    {
        this.item = item;
    }
}
