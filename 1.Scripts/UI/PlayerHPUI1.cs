using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPUI1 : MonoBehaviour
{
    public Transform PlayerHPLocation;
    //public Camera MainCamera;

    //private Vector3 screenPoint;
    Vector3 targetPos;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //screenPoint = Camera.main.WorldToScreenPoint(PlayerHPLocation.position);
        
        //rectTransform.position = Vector3.Lerp(rectTransform.position, screenPoint, 0.2f);
        //targetPos = new Vector3(PlayerHPLocation.position.x, PlayerHPLocation.position.y, 100) ;
        //rectTransform.position = targetPos;
        targetPos = PlayerHPLocation.position;
        rectTransform.position = targetPos;
    }
}
