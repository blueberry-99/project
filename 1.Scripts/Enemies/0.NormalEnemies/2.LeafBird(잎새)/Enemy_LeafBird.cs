using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LeafBird : Enemy
{
    public bool isGroundTouched;

    public Animator animator;
    public Animator grayScaledAnimator;
    public EnemyAI enemyAI;

    public Vector2 incomingVector;
    public Vector2 reflectedVector;

    private bool isplayerfound;

    void Awake()
    {
        InheritedAwake();
    }

    void Start()
    {
        InheritedStart();
        isplayerfound = false;

    }

    public override void InheritedAwake()
    {
        base.InheritedAwake();
    }

    public override void InheritedStart()
    {
        base.InheritedStart();
    }

    void Update()
    {
        if (isPlayerDetected)
        {
            if (!isplayerfound)
            {
                isplayerfound = true;
                grayScaledAnimator.Play("LeafBird_PlayerFind");
                animator.Play("LeafBird_PlayerFind");
                enemyAI.StartChase();
            }
        }

        if (isDie)
        {
            isDie = false;
            StartCoroutine(DieFade());
            for (int i = 0; i < 7; i++)
            {
                GameObjectPool.gameObjectPool.GetFromPool("StarCandy", transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator DieFade()
    {
        WaitForEndOfFrame wfef = new WaitForEndOfFrame();
        WaitForSeconds wfs = new WaitForSeconds(1);
        dieFadeValue = 1;

        while (dieFadeValue > 0)
        {
            dieFadeValue -= Time.deltaTime * 6;
            ColoredMaterial.SetFloat(fullGlowDissolveFadeID, dieFadeValue);
            yield return wfef;
        }
        yield return wfs;
        this.gameObject.SetActive(false);
    }

    /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGroundTouched = true;
            incomingVector = collision.GetContact(0).normal; // 입사 벡터
            reflectedVector = Vector2.Reflect(transform.up, incomingVector); // 반사 벡터
        }
    } */
}
