using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZSerializer;

public class PlayerManager : MonoBehaviour, IDataPersistence
{
    #region Player Variables

    [HideInInspector] public bool isInitialGame;

    [HideInInspector] public int currentGold;

    [HideInInspector] public int currentHP;
    [HideInInspector] public int maxHP; //Need To Be Loaded

    [HideInInspector] public int currentMP;
    [HideInInspector] public int maxMP; //Need To Be Loaded
    [HideInInspector] public int manaIncreaseAmount; //Need To Be Loaded
    private bool isManaFull;

    [HideInInspector] public float baseAttackDamage; //Need To Be Loaded
    [HideInInspector] public float skill_0_Range_Damage; //Need To Be Loaded

    [HideInInspector] public bool canDash; //Need To Be Loaded
    [HideInInspector] public bool canInAirDash; //Need To Be Loaded
    [HideInInspector] public bool canWallJump; //Need To Be Loaded
    [HideInInspector] public bool canSkill;

    [HideInInspector] public bool boss_CrazyFlower_Cleared; //Need To Be Loaded
    [HideInInspector] public bool boss_Queen_Cleared;

    [HideInInspector] public bool isGlassGained;

    [HideInInspector] public Vector2 lastSavedPosition;
    [HideInInspector] public string sceneToLoad;

    #endregion

    private Player player;
    public static PlayerManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Reset Current HP, MP
    }

    void Start()
    {
        //PlayerHPUI.instance.SetHP((int)currentHP);
    }

    //Scene이 로드될 때마다 LoadData가 호출됨 혹은, 최초 실행시 마다 LoadData가 호출됨.
    public void LoadData(GameData data)
    {

        if (instance != null)
        {
            Debug.Log("LoadData Called");
            #region Load Data From GameData
            isInitialGame = data.isInitialGame;
            sceneToLoad = data.sceneToLoad;
            lastSavedPosition = data.lastSavePoint;

            currentGold = data.currentGold;

            maxHP = data.maxHP;
            currentHP = data.currentHP;

            maxMP = data.maxMP;
            currentMP = data.currentMP;

            baseAttackDamage = data.baseAttackDamage;
            skill_0_Range_Damage = data.skill_0_Range_Damage;
            manaIncreaseAmount = data.manaIncreaseAmount;

            canDash = data.canDash;
            canInAirDash = data.canInAirDash;
            canWallJump = data.canWallJump;
            canSkill = data.canSkill;

            boss_CrazyFlower_Cleared = data.boss_CrazyFlower_Cleared;
            boss_Queen_Cleared = data.boss_Queen_Cleared;

            isGlassGained = data.isGlassGained;

            #endregion
            transform.position = data.lastSavePoint;

            //Debug.Log(currentMP);
            PlayerHPUI.instance.SetHP(currentHP);
            PlayerMPUI.instance.SetMP(currentMP);
        }
    }

    //씬이 언로드될 때마다 저장됨. 또는 특정 상황에(예: 세이브 포인트에 앉았을 때) 저장됨.
    public void SaveData(ref GameData data)
    {
        if (instance != null)
        {
            Debug.Log("SaveData Called");
            #region Load Data From GameData
            data.isInitialGame = isInitialGame;
            data.sceneToLoad = sceneToLoad;
            data.lastSavePoint = lastSavedPosition;

            data.currentGold = currentGold;

            data.maxHP = maxHP;
            data.currentHP = currentHP;

            data.maxMP = maxMP;

            data.currentMP = currentMP;

            data.baseAttackDamage = baseAttackDamage;
            data.skill_0_Range_Damage = skill_0_Range_Damage;
            data.manaIncreaseAmount = manaIncreaseAmount;
            data.canDash = canDash;
            data.canInAirDash = canInAirDash;
            data.canWallJump = canWallJump;
            data.canSkill = canSkill;

            data.boss_CrazyFlower_Cleared = boss_CrazyFlower_Cleared;
            data.boss_Queen_Cleared = boss_Queen_Cleared;

            data.isGlassGained = isGlassGained;

            #endregion
        }
    }

    public void GetGold(int amount)
    {
        currentGold += amount;
        PlayerGoldUI.instance.FadeIn();
        PlayerGoldUI.instance.SetGoldUI(currentGold);
    }

    public void PlayerHeal(int count)
    {
        currentHP += count;
        if (currentHP >= maxHP) currentHP = maxHP;
        PlayerHPUI.instance.SetHP(currentHP);
    }

    public void PlayerFullyHeal()
    {
        currentHP = maxHP;
        PlayerHPUI.instance.SetHP(currentHP);
    }

    public void PlayerDamaged(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        PlayerHPUI.instance.SetHP((int)currentHP);
        /* PlayerHPUI.DeleteHP(damage); */
    }

    private void PlayerDie()
    {

    }

    public void GetMana()
    {
        if (currentMP < maxMP)
        {
            currentMP += manaIncreaseAmount;
            if (currentMP >= maxMP)
            {
                if (!isManaFull)
                {
                    isManaFull = true;
                    PlayerSounds.instance.PlaySound("ManaFullSound");
                }
                currentMP = maxMP;
            }
            PlayerMPUI.instance.SetMP((int)currentMP);
        }
    }

    public bool IsManaFull()
    {
        if (currentMP == maxMP) return true;
        else return false;
        //return true;
        //for debugging
    }

    public void UseMana()
    {
        currentMP = 0;
        isManaFull = false;
        PlayerMPUI.instance.SetMP((int)currentMP);
        /* PlayerMPUI.SetMPUI(0); */
    }

    public void ShowSkillUI()
    {
        //PlayerSkillUI.ShowSkillUI();
    }

    public void HideSkillUI()
    {
        /* PlayerSkillUI.HideSkillUI(); */

    }

    public void ShowMiniMapUI()
    {
        /* PlayerMiniMapUI.ShowMiniMap(); */
    }

    public void HideMiniMapUI()
    {
        /* PlayerMiniMapUI.HideMiniMap(); */
    }
}
