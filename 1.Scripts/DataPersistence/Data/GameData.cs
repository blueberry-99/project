using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isInitialGame;
    public long lastUpdated;

    public int currentGold;

    public int maxHP;
    public int currentHP;

    public int maxMP;
    public int currentMP;
    public int manaIncreaseAmount;

    public float baseAttackDamage;
    public float skill_0_Range_Damage;

    public bool canDash;
    public bool canInAirDash;
    public bool canWallJump;
    public bool canSkill;

    public bool boss_CrazyFlower_Cleared;
    public bool boss_Queen_Cleared;

    public bool isGlassGained;

    public string sceneToLoad;
    public Vector3 lastSavePoint;

    public SerializableDictionary<string, bool> temp;

    public GameData()
    {
        isInitialGame = true;

        sceneToLoad = "May Lily Forest_0";
        lastSavePoint = new Vector3(-1, 0, 0);

        currentGold = 0;

        maxHP = 5;
        currentHP = 5;

        maxMP = 5;
        currentMP = 0;

        baseAttackDamage = 1;
        skill_0_Range_Damage = 2f;
        manaIncreaseAmount = 1;

        canDash = true;
        canInAirDash = true;
        canWallJump = true;
        canSkill = true;

        boss_CrazyFlower_Cleared = false;
        boss_Queen_Cleared = false;

        isGlassGained = false;

        temp = new SerializableDictionary<string, bool>();
    }
}
