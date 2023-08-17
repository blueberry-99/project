using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDieState : PlayerState
{
    private float dieAnimationTimeCounter;
    private float sceneTrastitionTimeCounter;
    private bool sceneLoaded;


    private float xForce;

    public PlayerDieState(Player player, PlayerStateMachine stateMachine, PlayerData playerData) : base(player, stateMachine, playerData)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.SetGravityScale(0);
        dieAnimationTimeCounter = 0.75f;
        sceneTrastitionTimeCounter = 1.7f;
        //Stop Player
        Player.RB.velocity = Vector2.zero;

        sceneLoaded = false;
        //Play Animation
        Player.PlayAnimation("Die");

        //Die Effect
        float angle = (Player.isFacingRight) ? 0 : 180;
        PlayerDetachedEffectPool.instance.GetFromPool("PlayerHitEffect", Quaternion.Euler(0, angle, -30));

        //Hit Freeze
        Player.FreezeTime(PlayerData.hitFreezeTime);

        //Play Sound
        PlayerSounds.instance.PlaySound("PlayerHitSound");

        xForce = (Player.isFacingRight) ? -0.5f : 0.5f;

    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        sceneTrastitionTimeCounter -= Time.deltaTime;
        dieAnimationTimeCounter -= Time.deltaTime;
        if (dieAnimationTimeCounter < 0)
        {
            FadeManager.fadeManager.FadeOut();
            Player.RB.velocity = Vector2.zero;
        }
        else
        {
            Player.RB.velocity = new Vector2(xForce, 0.5f);
        }
        //FadeOut 된 이후에,
        if (sceneTrastitionTimeCounter < 0)
        {
            if (!sceneLoaded)
            {
                sceneLoaded = true;

                //마지막으로 저장된 씬 로드
                SceneManager.LoadSceneAsync(PlayerManager.instance.sceneToLoad);
                //마지막으로 저장된 위치로 플레이어 이동
                Player.transform.position = PlayerManager.instance.lastSavedPosition;


                //만약 아직 한번도 저장장소에서 저장을 못했다면,
                if (PlayerManager.instance.isInitialGame)
                {
                    //세이브 포인트에서 부활 안하니깐 힐 다해주고
                    PlayerManager.instance.PlayerFullyHeal();
                    Player.WakeUpState.SetIsQuickWakeUp();
                    if (!Player.isFacingRight) Player.Flip();
                    StateMachine.ChangeState(Player.WakeUpState);
                }
                else
                {
                    //플레이어 저장 스테이트에게 부활한 것임을 저장.
                    StateMachine.ChangeState(Player.LayDownState);
                }
            }
        }
    }

    /* 
    TODO
    1. GameData에 저장된 마지막 Scene을 로드해야 함.
    2. 마지막 저장 데이터가 있으니깐, 그 위치로 이동 후, FadeManager FadeIn 하기
     */
}
