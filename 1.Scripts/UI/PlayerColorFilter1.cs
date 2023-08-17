using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorFilter1 : MonoBehaviour
{
    public Transform PlayerTransform;
    public RectTransform CanvasRect;

    public Camera CustomCamera;

    Animator Animator;
    RectTransform rectTransform;

    private Vector2 parentAnchor;

    public List<GameObject> GameObjects = new List<GameObject>();

    private Vector2[] GameObjectAnchors;

    int count;

    void Awake()
    {
        count = 0;
    }
    void Start()
    {
        //리스트 갯수만큼 동적 배열 생성
        GameObjectAnchors = new Vector2[GameObjects.Count];
        //
        StartCoroutine(Repeat());
    }

    IEnumerator Repeat()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.2f);
        while (true)
        {
            if (count >= GameObjects.Count)
            {
                count = 0;
                parentAnchor = Vector2.zero;
            }
            FollowPlayer(GameObjects[count]);
            count++;
            
            yield return wfs;
        }
    }

    void FollowPlayer(GameObject childGameObject)
    {
        Vector2 ViewportPosition = CustomCamera.WorldToViewportPoint(PlayerTransform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        Animator = childGameObject.GetComponent<Animator>();
        rectTransform = childGameObject.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = WorldObject_ScreenPosition - parentAnchor;

        Animator.Rebind();
        //Animator.Play("Appear&Disappear");

        parentAnchor += rectTransform.anchoredPosition;
    }
}
