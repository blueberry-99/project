using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrb : MonoBehaviour
{
    public Player Player;
    public Transform PlayerTransform;
    public float smoothTime;
    public float repeatTime;
    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;


    void Start()
    {
        //StartCoroutine(FollowPlayer(repeatTime));
    }

    void Update()
    {
        targetPos = PlayerTransform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        Debug.Log(Player.isFacingRight);
        if(Player.isFacingRight) transform.rotation = Quaternion.Euler(0,0,0);
        else transform.rotation = Quaternion.Euler(0,180,0);

    }

    IEnumerator FollowPlayer(float time)
    {
        WaitForSeconds wfs = new WaitForSeconds(time);
        while (true)
        {
            yield return wfs;
            targetPos = PlayerTransform.position;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothTime);
            //transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }

    }

}
