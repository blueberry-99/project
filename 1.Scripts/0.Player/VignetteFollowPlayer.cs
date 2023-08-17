using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteFollowPlayer : MonoBehaviour
{
    public Camera CustomCamera;
    public Transform player;  // 플레이어의 Transform 컴포넌트

    private Volume volume;
    private Vignette vignette;

    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 playerViewportPosition = CustomCamera.WorldToViewportPoint(player.position);
            vignette.center.value = playerViewportPosition;
        }
    }
}
