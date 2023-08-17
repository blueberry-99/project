using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStart : MonoBehaviour
{
    Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        StartCoroutine(RandomTime());
    }

    IEnumerator RandomTime()
    {
        WaitForSeconds wfs = new WaitForSeconds(Random.Range(0, 1f));
        yield return wfs;
        Animator.Play("PlayerColorMask");
    }
}
