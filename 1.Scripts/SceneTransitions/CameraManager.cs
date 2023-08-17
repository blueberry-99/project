using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    private Transform PlayerTransform;

    private void Awake()
    {

    }
    /* private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TeleportToPlayer();
    }

    void TeleportToPlayer()
    {
        PlayerTransform = PlayerManager.playerManager.GetComponent<Transform>();
        transform.position = PlayerTransform.position;
    } */
}
