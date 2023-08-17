using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStateMachine
{
    public Player Player;

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
    }

    public PlayerState CurrentState { get; private set; }
    
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();

        if (Player.PlayerData.debugState) Debug.Log(CurrentState.ToString());
    }
}
