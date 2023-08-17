using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/* using UnityEngine.SceneManagement; */

public class SetConfiner : MonoBehaviour
{
    GameObject CameraConfinderObject;
    CinemachineConfiner2D cinemachineConfiner2D;
    CinemachineVirtualCamera cvCam;
    PolygonCollider2D polygonCollider2D;

    private void Start()
    {
        cvCam = GetComponent<CinemachineVirtualCamera>();
        cvCam.Follow = PlayerManager.instance.transform;
    }
   /*  private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetConf();
    }

    private void SetConf()
    {
        CameraConfinderObject = GameObject.FindWithTag("CameraConfiner");
        polygonCollider2D = CameraConfinderObject.GetComponent<PolygonCollider2D>();
        cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
        cinemachineConfiner2D.m_BoundingShape2D = polygonCollider2D;
    } */
}
