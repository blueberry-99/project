using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMPUI : MonoBehaviour
{
    public Animator animator;

    public static PlayerMPUI instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetMP(int count)
    {
        animator.SetTrigger(count.ToString());
    }

}
