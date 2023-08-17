[System.Serializable]
public sealed class GameManagerZSerializer : ZSerializer.Internal.ZSerializer
{
    public GameManager gameManager;
    public System.Boolean isInitialData;
    public System.Boolean TitleToInGame;
    public UnityEngine.Events.UnityEvent CinemachineImpulse;
    public System.Int32 groupID;
    public System.Boolean autoSync;

    public GameManagerZSerializer(string ZUID, string GOZUID) : base(ZUID, GOZUID)
    {       var instance = ZSerializer.ZSerialize.idMap[ZSerializer.ZSerialize.CurrentGroupID][ZUID];
         gameManager = (GameManager)typeof(GameManager).GetField("gameManager").GetValue(instance);
         isInitialData = (System.Boolean)typeof(GameManager).GetField("isInitialData").GetValue(instance);
         TitleToInGame = (System.Boolean)typeof(GameManager).GetField("TitleToInGame").GetValue(instance);
         CinemachineImpulse = (UnityEngine.Events.UnityEvent)typeof(GameManager).GetField("CinemachineImpulse").GetValue(instance);
         groupID = (System.Int32)typeof(ZSerializer.PersistentMonoBehaviour).GetField("groupID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
         autoSync = (System.Boolean)typeof(ZSerializer.PersistentMonoBehaviour).GetField("autoSync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(instance);
    }

    public override void RestoreValues(UnityEngine.Component component)
    {
         typeof(GameManager).GetField("gameManager").SetValue(component, gameManager);
         typeof(GameManager).GetField("isInitialData").SetValue(component, isInitialData);
         typeof(GameManager).GetField("TitleToInGame").SetValue(component, TitleToInGame);
         typeof(GameManager).GetField("CinemachineImpulse").SetValue(component, CinemachineImpulse);
         typeof(ZSerializer.PersistentMonoBehaviour).GetField("groupID", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, groupID);
         typeof(ZSerializer.PersistentMonoBehaviour).GetField("autoSync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(component, autoSync);
    }
}