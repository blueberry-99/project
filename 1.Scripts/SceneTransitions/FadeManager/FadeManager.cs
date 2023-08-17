using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    Animator Animator;
    public static FadeManager fadeManager;

    public bool isSceneTrantition;
    void Awake()
    {
        if (fadeManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            fadeManager = this;
        }
        Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /* 
        기본을 FadeIn Slow로 하고, exitScene 에서는, fadeManager에 isInGameTrantition을 true
         */
        if (isSceneTrantition)
        {
            FadeIn();
            isSceneTrantition = false;
        }
        else
        {
            FadeInSlow();
        }
    }

    IEnumerator StartFadeIn(float time)
    {
        WaitForSecondsRealtime wfs = new WaitForSecondsRealtime(time);
        yield return wfs;
        Animator.Play("FadeIn");
    }

    public void FadeOut()
    {
        Animator.Play("FadeOut");
    }

    public void FadeIn()
    {
        Animator.Play("FadeIn");
    }

    public void FadeInSlow()
    {
        Animator.Play("FadeIn_Slow");
    }

    public void FadeInAfterTime(float time)
    {
        StartCoroutine(FadeInWithTime(time));
    }

    IEnumerator FadeInWithTime(float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        yield return wfs;
        Animator.Play("FadeIn");
    }
}
