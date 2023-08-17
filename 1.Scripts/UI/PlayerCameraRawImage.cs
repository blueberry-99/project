using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraRawImage : MonoBehaviour
{
    RectTransform rectTransform;
    public Camera CustomCamera;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    
    void LateUpdate()
    {
        rectTransform.position = CustomCamera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 99.999f));
    }
}
