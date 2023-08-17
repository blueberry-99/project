using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : PlayerState
{
    //Inputs
    private int xInput;
    private int yInput;

    //Skill Types
    private string skillID;

    const string ID_Skill_0 = "Sky : Range";
    const string ID_Skill_1 = "Red : Smash";


    public PlayerSkillState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //스킬 구분 후 분기
        switch (skillID)
        {
            case ID_Skill_0:
                StateMachine.ChangeState(Player.SKillState_0_Range);
                break;
            case ID_Skill_1:
                StateMachine.ChangeState(Player.SkillState_1_Smash);
                break;
        }
    }

    public void SetInput(int xInput, int yInput)
    {
        this.xInput = xInput;
        this.yInput = yInput;
    }

    //Called when skill changed, or when first skill learn
    public void SetSkillID(string temp)
    {
        skillID = temp;
    }
}


