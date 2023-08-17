using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBackWallCheck : MonoBehaviour
{
    public Player Player;

    private bool isToucingRightWall;
    private bool isToucingLeftWall;

    private void Awake()
    {
        isToucingLeftWall = false;
        Player.SetIsTouchingLeftWall(false);
        isToucingRightWall = false;
        Player.SetIsTouchingRightWall(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isToucingLeftWall = false;
        Player.SetIsTouchingLeftWall(false);
        isToucingRightWall = false;
        Player.SetIsTouchingRightWall(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //when enemy Hitted
        if (other.gameObject.tag == "Ground")
        {
            if (!Player.isFacingRight)
            {
                Player.SetIsTouchingRightWall(true);
                isToucingRightWall = true;
            }
            else
            {
                Player.SetIsTouchingLeftWall(true);
                isToucingLeftWall = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //when enemy Hitted
        if (other.gameObject.tag == "Ground")
        {
            if (isToucingRightWall)
            {
                isToucingRightWall = false;
                Player.SetIsTouchingRightWall(false);
            }
            else if (isToucingLeftWall)
            {
                isToucingLeftWall = false;
                Player.SetIsTouchingLeftWall(false);
            }

        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
