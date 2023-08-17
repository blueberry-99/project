using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorMask : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 fixedVector;
    [HideInInspector] public RectTransform CanvasRect;
    [HideInInspector] public Vector3 worldPosition;



    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = WorldObject_ScreenPosition;

    }

    public void DeleteEffect(float time)
    {
        Invoke(nameof(ReturnToPool), time);
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        PlayerColorMaskPool.instance.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
