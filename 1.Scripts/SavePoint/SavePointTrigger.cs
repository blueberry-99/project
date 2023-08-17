using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePointTrigger : MonoBehaviour
{
    [SerializeField] Transform SavePointPos;
    Player Player;
    public Vector2 savePointPosition;
    public string locatedSceneName;

    [SerializeField] Animator GuideTextAnimator;

    void Awake()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //SavePoint 트리거 안에 플레이어가 들어오면
        if (other.tag.Equals("Player"))
        {
            if (!other.TryGetComponent<Player>(out Player)) Debug.LogError("Player Componenet Missing");
            Player.SetCanSave(true);
            //요기까진 확인

            //현재 SavePoint Position이랑, 이 세이브 포인트가 있는 씬을 저장함.
            savePointPosition = SavePointPos.position;
            locatedSceneName = SceneManager.GetActiveScene().name;

            if(!Player.SaveState.isRevived) ShowGuideText();

            //플레이어에게 이 세이브포인트 트리거를 줌.
            Player.savePointTrigger = this;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Player.SetCanSave(false);
            HideGuideText();
        }
    }

    public void ShowGuideText()
    {
        GuideTextAnimator.Play("Show");
    }

    public void HideGuideText()
    {
        GuideTextAnimator.Play("Hide");
    }
}
