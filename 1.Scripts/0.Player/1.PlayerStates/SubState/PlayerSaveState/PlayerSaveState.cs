using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSaveState : PlayerState
{
    public Vector2 savePointPosition;
    private Vector2 velocity = Vector2.zero;

    private bool saveDone;

    private float saveTimeCounter;

    private string sceneName;

    public bool isRevived;

    public PlayerSaveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        saveDone = false;
        saveTimeCounter = 0.3f;

        sceneName = SceneManager.GetActiveScene().name;
        //Save Point에서 저장한 경우가 아닐 경우 (죽었다 부활하는 경우 혹은 처음 시작하는 경우)
        if (Player.savePointTrigger == null)
        {

            savePointPosition = PlayerManager.instance.lastSavedPosition;
        }
        else
        {
            savePointPosition = Player.savePointTrigger.savePointPosition;
        }

        Player.RB.velocity = new Vector2(0, 0);
        if (isRevived || GameManager.gameManager.TitleToInGame)
        {
            Player.PlayAnimation("Save");
            GameManager.gameManager.TitleToInGame = false;
        }
        else
        {
            Player.PlayAnimation("Save_Start");
        }

        if (Player.isFacingRight) Player.Flip();

        //SaveSystem.SavePlayer(PlayerStats.playerStats);
        Player.SetGravityScale(0);

        //Hide Guide Text
        if (Player.savePointTrigger != null) Player.savePointTrigger.HideGuideText();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        saveTimeCounter -= Time.deltaTime;
        if (saveTimeCounter < 0)
        {
            if (!saveDone)
            {
                saveDone = true;
                //Save Game
                PlayerManager.instance.lastSavedPosition = savePointPosition;
                PlayerManager.instance.sceneToLoad = SceneManager.GetActiveScene().name;
                PlayerManager.instance.isInitialGame = false;
                DataPersistenceManager.instance.SaveGame();
                StateMachine.ChangeState(Player.LayDownState);
            }
        }
        MoveTowardsSavePoint(savePointPosition);
    }

    private void MoveTowardsSavePoint(Vector2 position)
    {
        Player.transform.position = Vector2.SmoothDamp(Player.transform.position, position, ref velocity, 0.1f);
    }
}
