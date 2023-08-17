using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ExitScene : MonoBehaviour
{
    //public AudioMixerSnapshot snap;
    public BoxCollider2D boxCollider2D;
    public int ExitDir;
    public string sceneToLoad;
    public string exitName;

    private Animator FadeManagerAnimator;
    private Player Player;
    private int playerDir;

    private void Start()
    {
        if (ExitDir == 0) Debug.LogError("Set ExitDir");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            PlayerPrefs.SetString("LastExitName", exitName);
            Player = PlayerManager.instance.GetComponent<Player>();
            if (Player.InputHandler.DashInputHold) Player.InputHandler.StartCustomInput(ExitDir, false, true);
            else Player.InputHandler.StartCustomInput(ExitDir, false, false);
            DataPersistenceManager.instance.SaveGame();
            FadeManagerAnimator = FadeManager.fadeManager.GetComponent<Animator>();
            StartCoroutine(FadeOut());
            FadeManager.fadeManager.isSceneTrantition = true;
        }
    }

    IEnumerator FadeOut()
    {
        WaitForSecondsRealtime wfsR = new WaitForSecondsRealtime(0.33f);
        FadeManagerAnimator.Play("FadeOut");
        yield return wfsR;
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxCollider2D.size);
    }

}
