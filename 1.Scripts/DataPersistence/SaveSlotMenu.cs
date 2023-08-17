using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotMenu : MonoBehaviour
{
    [SerializeField] private Animator fadeManager_Title;
    CanvasGroup canvasGroup;
    private SaveSlot[] saveSlots;

    private string sceneToLoad;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.instance.ChangeSelectedProfileID(saveSlot.GetProfileID());
        if (saveSlot.isSaveSlotEmpty)
        {
            sceneToLoad = "May Lily Forest_0";
            DataPersistenceManager.instance.NewGame();
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            DataPersistenceManager.instance.LoadGame();
            DataPersistenceManager.instance.SaveGame();
            sceneToLoad = DataPersistenceManager.instance.gameData.sceneToLoad;
        }

        fadeManager_Title.Play("FadeOut");

        //캔버스 그룹 FadeOut, 화면 FadeOut 후  Scene Load
        StartCoroutine(FadeOut());
        StartCoroutine(CanvasGroupFadeOut(canvasGroup, 0.56f));
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileID());
        ActivateMenu();
    }

    public void ActivateMenu()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();

        Dictionary<string, GameData> profileGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profileGameData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

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
            //if (fadeValue <= 0) cg.gameObject.SetActive(false);
            yield return wfef;
        }
    }

    IEnumerator FadeOut()
    {
        WaitForSecondsRealtime wfsR = new WaitForSecondsRealtime(0.7f);
        yield return wfsR;
        SceneManager.LoadScene(sceneToLoad);
    }

}
