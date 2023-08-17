using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayDownState : PlayerState
{
    private int yInput;

    private float timeCounter;
    private bool saveEnd;

    public PlayerLayDownState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        saveEnd = false;

        timeCounter = 0.183f;

        //Player.RB.velocity = new Vector2(0, 0);
        Player.PlayAnimation("Save");

        Player.RB.velocity = Vector2.zero;
        if (Player.isFacingRight) Player.Flip();

        PlayerManager.instance.PlayerFullyHeal();

        //SaveSystem.SavePlayer(PlayerStats.playerStats);
        Player.SetGravityScale(0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Player.RB.velocity = Vector2.zero;
        yInput = Player.InputHandler.NormInputY;
        if (yInput == -1)
        {
            saveEnd = true;
        }
        if (saveEnd)
        {
            Player.PlayAnimation("Save_End");
            timeCounter -= Time.deltaTime;
            if (timeCounter < 0)
            {
                StateMachine.ChangeState(Player.InAirState);
            }
        }


    }

    public override void Exit()
    {
        base.Exit();
        Player.isInvincibe = false;
        Player.SetGravityScale(PlayerData.gravityScale);

        if (Player.savePointTrigger != null) Player.savePointTrigger.ShowGuideText();
    }
}
