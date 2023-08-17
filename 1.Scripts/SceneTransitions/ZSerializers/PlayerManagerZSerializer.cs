[System.Serializable]
public sealed class PlayerManagerZSerializer : ZSerializer.Internal.ZSerializer
{
    public PlayerManager instance;
    public System.Boolean isInitialGame;
    public System.Int32 currentGold;
    public System.Int32 currentHP;
    public System.Int32 maxHP;
    public System.Int32 currentMP;
    public System.Int32 maxMP;
    public UnityEngine.Vector2 lastSavedPosition;
    public System.Int32 manaIncreaseAmount;
    public System.Single baseAttackDamage;
    public System.Single skill_0_Range_Damage;
    public System.Boolean canDash;
    public System.Boolean canInAirDash;
    public System.Boolean canWallJump;
    public System.Boolean canSkill;
    public System.Boolean boss_CrazyFlower_Cleared;
    public System.Boolean boss_Queen_Cleared;
    public System.Boolean isGlassGained;
    public System.String sceneToLoad;
    public System.Int32 groupID;
    public System.Boolean autoSync;

    public PlayerManagerZSerializer(string ZUID, string GOZUID) : base(ZUID, GOZUID)
    {       var instance = ZSerializer.ZSerialize.idMap[ZSerializer.ZSerialize.CurrentGroupID][ZUID];
         instance = (PlayerManager)typeof(PlayerManager).GetField("instance").GetValue(instance);
         isInitialGame = (System.Boolean)typeof(PlayerManager).GetField("isInitialGame").GetValue(instance);
         currentGold = (System.Int32)typeof(PlayerManager).GetField("currentGold").GetValue(instance);
         currentHP = (System.Int32)typeof(PlayerManager).GetField("currentHP").GetValue(instance);
         maxHP = (System.Int32)typeof(PlayerManager).GetField("maxHP").GetValue(instance);
         currentMP = (System.Int32)typeof(PlayerManager).GetField("currentMP").GetValue(instance);
         maxMP = (System.Int32)typeof(PlayerManager).GetField("maxMP").GetValue(instance);
         lastSavedPosition = (UnityEngine.Vector2)typeof(PlayerManager).GetField("lastSavedPosition").GetValue(instance);
         manaIncreaseAmount = (System.Int32)typeof(PlayerManager).GetField("manaIncreaseAmount").GetValue(instance);
         baseAttackDamage = (System.Single)typeof(PlayerManager).GetField("baseAttackDamage").GetValue(instance);
         skill_0_Range_Damage = (System.Single)typeof(PlayerManager).GetField("skill_0_Range_Damage").GetValue(instance);
         canDash = (System.Boolean)typeof(PlayerManager).GetField("canDash").GetValue(instance);
         canInAirDash = (System.Boolean)typeof(PlayerManager).GetField("canInAirDash").GetValue(instance);
         canWallJump = (System.Boolean)typeof(PlayerManager).GetField("canWallJump").GetValue(instance);
         canSkill = (System.Boolean)typeof(PlayerManager).GetField("canSkill").GetValue(instance);
         boss_CrazyFlower_Cleared = (System.Boolean)typeof(PlayerManager).GetField("boss_CrazyFlower_Cleared").GetValue(instance);
         boss_Queen_Cleared = (System.Boolean)typeof(PlayerManager).GetField("boss_Queen_Cleared").GetValue(instance);
         isGlassGained = (System.Boolean)typeof(PlayerManager).GetField("isGlassGained").GetValue(instance);
         sceneToLoad = (System.String)typeof(PlayerManager).GetField("sceneToLoad").GetValue(instance);
         groupID = (System.Int32)typeof(ZSerializer.PersistentMonoBehaviour).GetField("groupID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         autoSync = (System.Boolean)typeof(ZSerializer.PersistentMonoBehaviour).GetField("autoSync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
    }

    public override void RestoreValues(UnityEngine.Component component)
    {
         typeof(PlayerManager).GetField("instance").SetValue(component, instance);
         typeof(PlayerManager).GetField("isInitialGame").SetValue(component, isInitialGame);
         typeof(PlayerManager).GetField("currentGold").SetValue(component, currentGold);
         typeof(PlayerManager).GetField("currentHP").SetValue(component, currentHP);
         typeof(PlayerManager).GetField("maxHP").SetValue(component, maxHP);
         typeof(PlayerManager).GetField("currentMP").SetValue(component, currentMP);
         typeof(PlayerManager).GetField("maxMP").SetValue(component, maxMP);
         typeof(PlayerManager).GetField("lastSavedPosition").SetValue(component, lastSavedPosition);
         typeof(PlayerManager).GetField("manaIncreaseAmount").SetValue(component, manaIncreaseAmount);
         typeof(PlayerManager).GetField("baseAttackDamage").SetValue(component, baseAttackDamage);
         typeof(PlayerManager).GetField("skill_0_Range_Damage").SetValue(component, skill_0_Range_Damage);
         typeof(PlayerManager).GetField("canDash").SetValue(component, canDash);
         typeof(PlayerManager).GetField("canInAirDash").SetValue(component, canInAirDash);
         typeof(PlayerManager).GetField("canWallJump").SetValue(component, canWallJump);
         typeof(PlayerManager).GetField("canSkill").SetValue(component, canSkill);
         typeof(PlayerManager).GetField("boss_CrazyFlower_Cleared").SetValue(component, boss_CrazyFlower_Cleared);
         typeof(PlayerManager).GetField("boss_Queen_Cleared").SetValue(component, boss_Queen_Cleared);
         typeof(PlayerManager).GetField("isGlassGained").SetValue(component, isGlassGained);
         typeof(PlayerManager).GetField("sceneToLoad").SetValue(component, sceneToLoad);
         typeof(ZSerializer.PersistentMonoBehaviour).GetField("groupID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, groupID);
         typeof(ZSerializer.PersistentMonoBehaviour).GetField("autoSync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, autoSync);
    }
}