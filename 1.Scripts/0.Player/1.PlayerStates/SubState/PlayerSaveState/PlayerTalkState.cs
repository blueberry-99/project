using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkState : PlayerState
{
    private bool isArrived;
    private bool isTalkFinished;
    //private bool isTalkFinishAnimationPlayed;


    public Item talkStateItem;

    private bool jumpInput;

    private int yInput;

    private float talkFinishCounter;

    private Vector2 talkPosition;

    public bool isItemGet;
    public int itemDialogCount;

    public PlayerTalkState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.isInvincibe = true;
        isItemGet = false;
        isArrived = false;
        isTalkFinished = false;
        talkFinishCounter = 0.183f;

        if (Mathf.Abs(talkPosition.x - Player.transform.position.x) < 0.2f)
        {
            if (!Player.isFacingRight) Player.Flip();
        }
        else if (talkPosition.x + 0.2f < Player.transform.position.x)
        {
            Player.PlayAnimation("Bend");
            if (Player.isFacingRight) Player.Flip();
            Player.RB.velocity = new Vector2(-PlayerData.maxMoveSpeed, 0);
        }
        else if (talkPosition.x - 0.2f > Player.transform.position.x)
        {
            Player.PlayAnimation("Bend");
            if (!Player.isFacingRight) Player.Flip();
            Player.RB.velocity = new Vector2(PlayerData.maxMoveSpeed, 0);
        }

        Player.npc.HideGuide();
        //Hide HP & MP UI
        PlayerHPMPManager.instance.HideUI();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        yInput = Player.InputHandler.NormInputY;
        jumpInput = Player.InputHandler.JumpInput;

        if (!isArrived)
        {
            if (Mathf.Abs(talkPosition.x - Player.transform.position.x) < 0.2f)
            {
                Player.RB.velocity = Vector2.zero;
                isArrived = true;
                Player.PlayAnimation("Talk_Start");

                if (Player.npc.istalkRight)
                {
                    if (Player.isFacingRight) Player.Flip();
                }
                else
                {
                    if (!Player.isFacingRight) Player.Flip();
                }

                //Start Talking
                Player.npc.TalkStart();
                DialogueUI.instance.StartDialogue(Player.npc.dialogue);
            }
        }
        else
        {
            if (jumpInput)
            {
                Player.InputHandler.UseJumpInput();
                DialogueUI.instance.DisplayNextSentence();
                if (isItemGet)
                {
                    if (DialogueUI.instance.sentences.Count.Equals(itemDialogCount))
                    {
                        isItemGet = false;
                        talkStateItem.PlayerGetItem();
                    }
                }
            }
            if (isTalkFinished)
            {
                if (talkFinishCounter >= 0) talkFinishCounter -= Time.deltaTime;
                else
                {
                    StateMachine.ChangeState(Player.IdleState);
                }
            }
        }



    }

    public void DialogEnded()
    {
        isTalkFinished = true;
        Player.PlayAnimation("Talk_End");
        Player.npc.TalkFinished();
    }

    public override void Exit()
    {
        base.Exit();
        PlayerHPMPManager.instance.ShowUI();
        Player.isInvincibe = false;
        isArrived = false;
        isTalkFinished = false;
    }

    public void SetTalkPosition(Vector2 pos) => talkPosition = pos;
}
