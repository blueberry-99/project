using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class EntranceScene : MonoBehaviour
{
    //public AudioMixerSnapshot snap;
    public int EntranceDir;
    public string lastExitName;

    private Player Player;
    private Animator FadeManagerAnimator;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Lifecycle Method : EX > 나중에 세이브 포인트에서 저장하는 방식으로 변경
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeManagerAnimator = FadeManager.fadeManager.GetComponent<Animator>();
        if (EntranceDir == 0) Debug.LogError("Set EntranceDir");

        if (PlayerPrefs.GetString("LastExitName") == lastExitName)
        {

            PlayerManager.instance.transform.position = transform.position;
            Player = PlayerManager.instance.GetComponent<Player>();
            if (Player.InputHandler.DashInputHold) Player.InputHandler.StartCustomInput(EntranceDir, false, true);
            else Player.InputHandler.StartCustomInput(EntranceDir, false, false);

            Player.InputHandler.CanPlayerInput(0.2f);
            //Reset LastExitName
            PlayerPrefs.SetString("LastExitName", null);
            
            //FadeManagerAnimator.Play("FadeIn");
        }
    }

/*     IEnumerator FadeIn()
    {
        WaitForSecondsRealtime wfsR = new WaitForSecondsRealtime(0.5f);
        FadeManagerAnimator.Play("FadeIn");
        yield return wfsR;
    } */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
