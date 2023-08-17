using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using MaskIntr = UnityEngine.SpriteMaskInteraction;

public class PlayerColorFilter : MonoBehaviour
{
    public Transform PlayerTransform;
    public RectTransform CanvasRect;

    public Camera CustomCamera;
    RectTransform rectTransform;
    //
    Vector2 temp;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        FollowPlayer();
        
    }

    void FollowPlayer()
    {
        Vector2 ViewportPosition = CustomCamera.WorldToViewportPoint(PlayerTransform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }
}
