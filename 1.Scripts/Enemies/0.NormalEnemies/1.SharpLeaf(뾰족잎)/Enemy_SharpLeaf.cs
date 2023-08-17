using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SharpLeaf : Enemy
{
    Animator animator;
    public bool isAttackDash;

    void Awake()
    {
        InheritedAwake();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        InheritedStart();
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
        if (isDie)
        {
            EnemyDie();
            isDie = false;
        }
    }
    void EnemyDie()
    {
        animator.SetTrigger("die");
        StartCoroutine(DieFade());
        for (int i = 0; i < 10; i++)
        {
            GameObjectPool.gameObjectPool.GetFromPool("StarCandy", transform.position, Quaternion.identity);
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

}
