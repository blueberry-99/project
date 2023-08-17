using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ClownFlower : Enemy
{
    private Animator grayScaledAnimator;
    private Animator animator;

    [SerializeField] BossTrigger bossTrigger;
    [SerializeField] GameObject SpawnItemObject;
    [SerializeField] Transform ItemSpawnPos;

    [HideInInspector] public bool isJumped;
    [HideInInspector] public bool isRunStarted;

    [HideInInspector] public bool runstop;
    [HideInInspector] public bool land;

    public Transform DashEffectPos;
    public Transform InAirDashEffectPos;
    
    void Awake()
    {
        InheritedAwake();
        animator = GetComponent<Animator>();
        grayScaledAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        InheritedStart();
    }

    void Update()
    {
        if (isDie)
        {
            BossDie();
            isDie = false;
        }
    }

    private void BossDie()
    {
        //SpawnItemObject.transform.position = new Vector2(ItemSpawnPos.position.x ,ItemSpawnPos.position.y + 0.3f); 
        SpawnItemObject.SetActive(true);
        bossTrigger.BossFightEnd();
        SetTrigger("die");
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
        gameObject.SetActive(false);
    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
        grayScaledAnimator.SetTrigger(name);
    }

    public override void InheritedAwake()
    {
        base.InheritedAwake();
    }

    public override void InheritedStart()
    {
        base.InheritedStart();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isJumped)
        {
            if (other.collider.tag == "Ground")
            {
                for (int i = 0; i < other.contactCount; i++)
                {
                    if (other.GetContact(i).normal == Vector2.up)
                    {
                        SetTrigger("land");

                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isRunStarted)
        {
            if (other.collider.tag == "Ground")
            {
                for (int i = 0; i < other.contactCount; i++)
                {
                    if (other.GetContact(i).normal == Vector2.left || other.GetContact(i).normal == Vector2.right)
                    {
                        SetTrigger("runstop");
                        isRunStarted = false;
                        break;
                    }
                }
            }
        }
    }
}
