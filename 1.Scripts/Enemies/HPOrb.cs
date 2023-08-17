using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPOrb : MonoBehaviour
{
    public Transform PlayerPos;
    float timeCounter;

    private float spreadTime;
    private float spreadDirX;
    public float minSpreadTime;
    public float maxSpreadTime;
    public float spreadSpeed;

    public float waitTime;
    private float waitTimeCounter;

    Vector2 spreadDir;
    Vector2 targetVector;
    Vector2 velocity = Vector2.zero;

    void OnEnable()
    {
        spreadDirX = Random.Range(-0.5f, 0.5f);
        spreadTime = Random.Range(minSpreadTime, maxSpreadTime);
        StartCoroutine(Spread(spreadTime));
        //위쪽 랜덤한 각도로 퍼지기
        spreadDir = new Vector2(spreadDirX, 1);

        waitTimeCounter = waitTime;
    }

    IEnumerator Spread(float time)
    {
        WaitForEndOfFrame wfef = new WaitForEndOfFrame();

        timeCounter = time;
        while (timeCounter > 0)
        {
            timeCounter -= Time.deltaTime;
            spreadDir = Vector2.SmoothDamp(spreadDir, targetVector, ref velocity, spreadTime * 0.5f);
            transform.Translate(spreadDir * spreadSpeed * Time.deltaTime);
            yield return wfef;
        }
    }

    void Update()
    {
        if (timeCounter <= 0)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                transform.position = Vector2.SmoothDamp(transform.position, PlayerPos.position, ref velocity, 0.1f);
            }
        }
    }
}
