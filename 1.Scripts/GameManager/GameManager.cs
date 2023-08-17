using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZSerializer;


public class GameManager : PersistentMonoBehaviour
{
    public static GameManager gameManager;

    public bool isInitialData;

    public bool TitleToInGame;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UnityEvent CinemachineImpulse;

    public void CameraShake()
    {
        CinemachineImpulse.Invoke();
    }
}
