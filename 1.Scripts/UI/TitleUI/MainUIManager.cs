using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [SerializeField] private Animator fadeManagerAnimator;

    public float transtitionFadeTime = 0.3f;
    public CanvasGroup titleCanvasGroup;
    public CanvasGroup profileCanvasGroup;
    public CanvasGroup optionsCanvasGroup;

    public void OnNewGameClicked()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("0. StartingPoint");
    }

    public void OnLoadGameClicked()
    {
        TitleFadeOut_ProFileFadeIn();
        saveSlotMenu.ActivateMenu();
    }

    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {

    }

    public void TitleFadeOut_ProFileFadeIn()
    {
        TitleFadeOut();
        Invoke(nameof(ProFileFadeIn), 0.5f);
    }

    public void TitleFadeOut_OptionsFadeIn()
    {
        TitleFadeOut();
        Invoke(nameof(OptionFadeIn), 0.5f);
    }

    public void ProFileFadeOut_TitleFadeIn()
    {
        ProFileFadeOut();
        Invoke(nameof(TitleFadeIn), 0.5f);
    }

    public void OptionFadeOut_TitleFadeIn()
    {
        OptionFadeOut();
        Invoke(nameof(TitleFadeIn), 0.5f);
    }

    public void TitleFadeIn() => StartCoroutine(CanvasGroupFadeIn(titleCanvasGroup, transtitionFadeTime));

    public void TitleFadeOut() => StartCoroutine(CanvasGroupFadeOut(titleCanvasGroup, transtitionFadeTime));

    public void ProFileFadeIn() => StartCoroutine(CanvasGroupFadeIn(profileCanvasGroup, transtitionFadeTime));

    public void ProFileFadeOut() => StartCoroutine(CanvasGroupFadeOut(profileCanvasGroup, transtitionFadeTime));

    public void OptionFadeIn() => StartCoroutine(CanvasGroupFadeIn(optionsCanvasGroup, transtitionFadeTime));

    public void OptionFadeOut() => StartCoroutine(CanvasGroupFadeOut(optionsCanvasGroup, transtitionFadeTime));

    IEnumerator CanvasGroupFadeOut(CanvasGroup cg, float fadeTime)
    {
        WaitForEndOfFrame wfef = new WaitForEndOfFrame();
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(0.1f);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        yield return wfsr;

        float fadeValue = 1;
        while (fadeValue > 0)
        {
            fadeValue -= Time.deltaTime / fadeTime;
            cg.alpha = fadeValue;
            if (fadeValue <= 0) cg.gameObject.SetActive(false);
            yield return wfef;
        }
    }

    public void OnExitButtonClicked()
    {
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        titleCanvasGroup.interactable = false;
        titleCanvasGroup.blocksRaycasts = false;
        WaitForSecondsRealtime wfsr = new WaitForSecondsRealtime(0.7f);
        fadeManagerAnimator.Play("FadeOut");
        yield return wfsr;
        titleCanvasGroup.gameObject.SetActive(false);
        Application.Quit();
    }

    IEnumerator CanvasGroupFadeIn(CanvasGroup cg, float fadeTime)
    {
        cg.gameObject.SetActive(true);
        cg.interactable = true;
        cg.blocksRaycasts = true;

        float fadeValue = 0;
        WaitForEndOfFrame wfef = new WaitForEndOfFrame();
        while (fadeValue < 1)
        {
            fadeValue += Time.deltaTime / fadeTime;
            cg.alpha = fadeValue;
            yield return wfef;
        }
    }


}
